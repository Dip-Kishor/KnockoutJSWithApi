
function ItemViewModel(data) {
    var self = this;
    self.id = ko.observable(data.id || 0);
    self.name = ko.observable(data.name || '');
    self.price = ko.observable(data.price || 0);
    self.file = ko.observable(null);
    self.isPublished = ko.observable(data.isPublished || false);
    self.imageUrl = ko.observable(data.imageUrl || '');
    self.showImage = ko.observable(false);
}


ItemViewModel.fromServerModel = function (data) {
    return new ItemViewModel(data)
};
