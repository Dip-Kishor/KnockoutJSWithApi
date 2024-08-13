function ItemController() {
    var self = this;
    self.items = ko.observableArray([]);
    self.newItemName = ko.observable('');
    self.newItemPrice = ko.observable('');
    self.newItemIsPublished = ko.observable('');


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
                alert('Failed to fetch items');
            }
        });
    };

    // Add a new item
    self.addItem = function () {
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
            },
            error: function () {
                alert('Failed to add item');
            }
        });
    };

    // Update an item
    self.updateItem = function (item) {
        $.ajax({
            url: 'api/item/' + item.id(),
            type: 'PUT',
            data: ko.toJSON(item),
            contentType: 'application/json',
            success: function () {
                alert('Item updated successfully');
            },
            error: function () {
                alert('Failed to update item');
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
                    alert('Failed to delete item');
                }
            });
        }
    };
}
