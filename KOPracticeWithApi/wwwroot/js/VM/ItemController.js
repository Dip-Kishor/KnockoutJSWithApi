function ItemController() {
    var self = this;

    self.items = ko.observableArray([]);
    self.newItemName = ko.observable('');
    self.newItemPrice = ko.observable('');
    self.newItemIsPublished = ko.observable(false);
    self.selectedItem = ko.observable(new ItemViewModel({}));
    self.selectedFile = ko.observable(null);
    self.successMessage = ko.observable('');
    self.successItemAddMessage = ko.observable('');
    self.failureMessage = ko.observable('');
    self.searchQuery = ko.observable('');
    self.currentPage = ko.observable(1);
    self.itemsPerPage = 10;
    self.nameError = ko.observable('');
    self.priceError = ko.observable('');
    self.selectedImageUrl = ko.observable('');


    

    self.searchQueryThrottled = ko.pureComputed(self.searchQuery)
        .extend({ rateLimit: { timeout: 300, method: "notifyWhenChangesStop" } });

    
    self.filteredItems = ko.computed(function () {
        var query = self.searchQueryThrottled().toLowerCase();

        if (!query) {
            return self.items();
        } else {
            return ko.utils.arrayFilter(self.items(), function (item) {
                return item.name().toLowerCase().includes(query);
            });
        }
    });

    
    self.totalPages = ko.computed(function () {
        return Math.ceil(self.filteredItems().length / self.itemsPerPage);
    });

    
    self.pagedItems = ko.computed(function () {
        var startIndex = (self.currentPage() - 1) * self.itemsPerPage;
        return self.filteredItems().slice(startIndex, startIndex + self.itemsPerPage);
    });

    
    self.goToPage = function (pageNumber) {
        if (pageNumber > 0 && pageNumber <= self.totalPages()) {
            self.currentPage(pageNumber);
        }
    };

    self.nextPage = function () {
        if (self.currentPage() < self.totalPages()) {
            self.currentPage(self.currentPage() + 1);
        }
    };

    self.previousPage = function () {
        if (self.currentPage() > 1) {
            self.currentPage(self.currentPage() - 1);
        }
    };

    self.itemsCount = ko.computed(function () {
        return self.items().length;
    });

    
    self.setSuccessMessage = function (msg) {
        self.successMessage(msg);
        setTimeout(function () {
            self.successMessage('');
        }, 5000);
    };
    self.setSuccessItemAddMessage = function (msg) {
        self.successItemAddMessage(msg);
        setTimeout(function () {
            self.successItemAddMessage('');
        }, 4000);
    };

    self.setFailureMessage = function (msg) {
        self.failureMessage(msg);
        setTimeout(function () {
            self.failureMessage('');
        }, 5000);
    };


    self.openImageModal = function (imageUrl) {
        self.selectedImageUrl(imageUrl());
        $('#imageModal').modal('show');
    };
    // Fetching all items from the server
    self.fetchItems = function () {
        $.ajax({
            url: 'api/item',
            type: 'GET',
            success: function (data) {
                var mappedItems = $.map(data, function (itemData) {
                    return ItemViewModel.fromServerModel(itemData);
                });
                self.items(mappedItems);
            },
            error: function () {
                self.setFailureMessage('Failed to fetch items');
            }
        });
    };



    // Fetch a single item by ID
    self.fetchItemById = function (itemId) {
        $.ajax({
            url: 'api/item/' + itemId,
            type: 'GET',
            success: function (data) {
                var item = ItemViewModel.fromServerModel(data);
                self.selectedItem(item);
                $('#itemDetailsModal').modal('show');
            },
            error: function () {
                self.setMessage('Failed to fetch the item');
            }
        });
    };

    // Add a new item
    /*self.addItem = function () {
        var name = self.newItemName().trim();
        var price = parseFloat(self.newItemPrice());

        self.nameError('');
        self.priceError('');

        if (!name) {
            self.nameError('Name cannot be empty');
            return;
        }
        if (!isNaN(name)) {
            self.nameError('Name cannot be a number');
            return;
        }
        if (isNaN(price) || price <= 0) {
            self.priceError('Price must be a positive number');
            return;
        }

        var newItem = new ItemViewModel({
            name: self.newItemName(),
            price: self.newItemPrice(),
            isPublished: self.newItemIsPublished()
        });

        $.ajax({
            url: 'api/item',
            type: 'POST',
            data: ko.toJSON(newItem),
            contentType: 'application/json',
            success: function () {
                self.fetchItems();
                self.newItemName('');
                self.newItemPrice('');
                self.newItemIsPublished(false);
                
                self.setSuccessItemAddMessage('Item added successfully!');
                $('#addItemModal').modal('hide');
            },
            error: function () {
                self.setFailureMessage('Failed to add item');
            }
        });
    };*/
    self.fileSelected = function (event) {
        var files = event.target.files;
        if (files.length > 0) {
            self.selectedFile(files[0]);
        } else {
            self.selectedFile(null);
        }
    };

    self.addItem = function () {
        var name = self.newItemName().trim();
        var price = parseFloat(self.newItemPrice());

        self.nameError('');
        self.priceError('');

        if (!name) {
            self.nameError('Name cannot be empty');
            return;
        }
        if (!isNaN(name)) {
            self.nameError('Name cannot be a number');
            return;
        }
        if (isNaN(price) || price <= 0) {
            self.priceError('Price must be a positive number');
            return;
        }

        var formData = new FormData();
        formData.append('name', self.newItemName());
        formData.append('price', self.newItemPrice());
        formData.append('isPublished', self.newItemIsPublished());
        var fileInput = document.getElementById('fileInput'); 
        var file = fileInput.files[0];
        if (file) {
            formData.append('ImageFile', file);
        } else {
            formData.append('ImageFile', ''); 
        }

        $.ajax({
            url: 'api/item',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                self.fetchItems();
                self.newItemName('');
                self.newItemPrice('');
                self.newItemIsPublished(false);
                fileInput.value = '';
                self.setSuccessItemAddMessage('Item added successfully!');
                $('#addItemModal').modal('hide');
            },
            error: function (xhr) {
                var response = JSON.parse(xhr.responseText);
                console.error('Failed to add item:', response);
                self.setFailureMessage('Failed to add item');
            }
        });
    };


    // File input change handler
    self.fileSelected = function (element) {
        var file = element.files[0];
        self.selectedFile(file);
    };

    // Edit an item
    self.editItem = function (item) {
        self.selectedItem(item);
        $('#updateItemModal').modal('show');
    };

    // Update an item
    self.updateItem = function () {
        let item = self.selectedItem();
        let itemId = item.id();

        
        if (!itemId) {
            return;
        }

        let name = item.name().trim();
        let price = parseFloat(item.price());
        let isPublished = item.isPublished();
        self.nameError('');
        self.priceError('');
        

        if (!name) {
            self.nameError('Name cannot be empty');
            return;
        }
        if (!isNaN(name)) {
            self.nameError('Name cannot be a number');
            return;
        }
        if (isNaN(price) || price <= 0) {
            self.priceError('Price must be a positive number');
            return;
        }

        
        $.ajax({
            url: `/api/item/${itemId}`,
            type: 'PUT',
            data: ko.toJSON(item),
            contentType: 'application/json',
            success: function (result) {
                
                let updatedItemIndex = self.items().findIndex(i => i.id() === itemId);
                if (updatedItemIndex > -1) {
                    self.items()[updatedItemIndex].name(item.name());
                    self.items()[updatedItemIndex].price(item.price());
                    self.items()[updatedItemIndex].isPublished(item.isPublished());
                }
                $('#itemDetailsModal').modal('hide');
                self.setSuccessMessage('Item updated successfully!');
            },
            error: function () {
                self.setFailureMessage('Failed to update item');
            }
        });
    };

    // Delete an item
    self.deleteItem = function (item) {
        var confirmDelete = confirm("Are you sure you want to delete this item?");

        if (confirmDelete) {
            $.ajax({
                url: '/api/item/' + item.id(),
                type: 'DELETE',
                success: function () {
                    self.items.remove(item);
                },
                error: function () {
                    self.setFailureMessage('Failed to delete item');
                }
            });
        }
    };
}
