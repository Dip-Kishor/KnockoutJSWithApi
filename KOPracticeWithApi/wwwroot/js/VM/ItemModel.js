
function ItemViewModel(data) {
    var self = this;
    self.id = ko.observable(data.id || 0);
    self.name = ko.observable(data.name || '');
    self.price = ko.observable(data.price || 0);
    self.isPublished = ko.observable(data.isPublished);
}


ItemViewModel.fromServerModel = function (data) {
    return new ItemViewModel({
        id: data.id,
        name: data.name,
        price: data.price,
        isPublished: data.isPublished
    });
};
