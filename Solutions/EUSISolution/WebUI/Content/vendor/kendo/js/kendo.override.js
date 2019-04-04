kendo.ui.FilterMenu.prototype.filter = function (expression) {
    expression = this._merge(expression);
    if (expression.filters.length) {
        this.dataSource.filter(expression);
    } else {
        this.dataSource.filter({});
    }
}