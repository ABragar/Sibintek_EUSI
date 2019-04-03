
var modelTrigger = kendo.observable();

var ViewModeldefinition = kendo.data.Model.define({
	newObject: function (model, e) {
		var result = new ViewModeldefinition();

		result.setModel(model, false);


		if (e) {
			result.fromObject(e);
		} else {
			result.createDefault();
		}
		return result;
	},



	createDefault: function () {

		var that = this;

		$.each(this.model.values, function (i, model) {
			that.set(model.id, null);
		});

		$.each(this.model.items, function (i, model) {

			that.set(model.id, [that.newObject(model)]);

		});

	},

	setModel: function (model, root) {

		this.model = model;

		var that = this;

		var actions = [];

		var action_name;

		if (root === false) {

			action_name = "addToParent";
			this[action_name] = function () {

				var parent = this.parent();
				parent.push(this.newObject(model));

				modelTrigger.trigger("add");
			};
			actions.push({ text: "Добавить \"" + model.name + "\"", action: action_name });

			action_name = "removeFromParent";

			this[action_name] = function () {

				var parent = this.parent();

				var index = parent.indexOf(this);
				parent.splice(index, 1);

			},

			actions.push({ text: "Удалить этот элемент", action: action_name });

		}

		$.each(model.items, function (i, model) {

			action_name = "add" + model.id;

			that[action_name] = function () {
			    
			    this.get(model.id).push(this.newObject(model));

			    modelTrigger.trigger("add");

			};
			actions.push({ text: "Добавить \"" + model.name + "\"", action: action_name });

		});

		this.actions = actions;
	},


	toObject: function (e) {


		var result = {};

		if ($.isPlainObject(e)) {
			$.extend(result, e);
		}
		if (!$.isPlainObject(result.items)) {
			result.items = {};
		}
		if (!$.isPlainObject(result.values)) {
			result.values = {};
		}


		var that = this;

		$.each(this.model.values, function (i, model) {
			var obj = that.get(model.id);

			result.values[model.name] = (obj instanceof kendo.data.ObservableObject) ? obj.toJSON() : obj;


		});

		$.each(this.model.items, function (i, model) {

			result.items[model.name] = $.map(that.get(model.id), function (e) { return e.toObject(result.items[model.name]); });

		});

		return result;
	},

	defaultFromObject: function(e) {
		var values = e.values ? e.values : {};

		var that = this;

		$.each(this.model.values, function (i, model) {

			that.set(model.id, values[model.name]);


		});

		var items = e.items ? e.items : {};
		$.each(this.model.items, function (i, model) {

			var array = items[model.name] ? items[model.name] : [null];

			that.set(model.id, $.map(array, function (e) {

				return that.newObject(model, e);

			}));


		});
	},

	fromObject: function (e) {

		var values = e.values ? e.values : {};

		var that = this;

		$.each(this.model.values, function (i, model) {

			that.set(model.id, values[model.name]);


		});

		var items = e.items ? e.items : {};
		$.each(this.model.items, function (i, model) {

			var array = items[model.name] ? items[model.name] : [];

			that.set(model.id, $.map(array, function (e) {

				return that.newObject(model, e ? e : {});

			}));




		});
	}
});




var menuDefinition = function (menu, target) {
	$(menu).kendoContextMenu({
		target: target,
		filter: "[data-uid]",
		open: function (e) {

			var obj = e.target.kendoBindingTarget.source;

			this.remove(".k-item");

			this.append(obj.actions);

		},
		select: function (e) {


			var index = $(e.item).index();

			var obj = e.target.kendoBindingTarget.source;

			obj[obj.actions[index].action]();
		}
	});
};