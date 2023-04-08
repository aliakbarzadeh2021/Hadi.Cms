Namespace('custom.ui').Treeviwe = new function(){
		var _selectedId = null;
		var _selectedName = null;
		var _items = null;

		var _onSelect = function (e){
			var treeview = $("#treeview").data("kendoTreeView");
			var _item = treeview.dataItem(e.node);
			var text = treeview.text(e.node);

			//e.preventDefault();
			if (_item.Type !== "Category") {
				_selectedId = _item.id;
				_selectedName = text;
				_items = _item;
				$('#showtext').html('<div class="treeviewselectedtext">' + text + '</div>');
			}
			else
			{
				_selectedId = null
				_selectedName = null;
				_items = null;
				$('#showtext').html('<div class="treeviewselectedtext">Please select a valid item</div>');
			}
		}
		var _createmodal = function (resultHandler){
			var _treemodalhtml = '<div id="treediv"><div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">' +
				'<div class="modal-dialog" role="document">' +
				'<div class="modal-content">' +
				'<div class="modal-header">' +
				'<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
				'<div class="modal-title" id="myModalLabel"><div id="showtext" class="treeviewheadtext">Please select a valid item</div>' +
				'</div>' +
				'<div class="modal-body">' +
				'<div id="treeview"style="psition:relative;height: 400px;overflow-y:auto;"></div>' +
				'</div>' +
				'<div class="modal-footer">' +
				'<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
				'<button type="button" class="btn btn-primary" id="sendformtemplate">Select</button>' +
				'</div>' +
				'</div>' +
				'</div>' +
				'</div></div>';
				var dialogContent = $(_treemodalhtml);
				$('body').append(dialogContent);
			//Select Button
			$("#sendformtemplate").click(function() {
				//need check selected node here if has children
				if(_selectedId !== null){
				resultHandler(_selectedId,_selectedName,_items);
				$('#myModal').modal('hide');
				//Remove modal-backdrop fade in 
				$("#myModal").on('hidden.bs.modal', function () {
					$(this).data('bs.modal', null);
				});
				}
			});
			//Modal Close
			$('#myModal').on('hidden.bs.modal', function (e) {
					_selectedId = null;
					_selectedName = null;
					$('#showtext').html('<div class="treeviewselectedtext">Please select a valid item</div>');
					$('#treediv').remove();
			});
		}
		var _showTreeObjectPicker = function (dataUrl, resultHandler){
			_createmodal(resultHandler);
			var dataSource = new kendo.data.HierarchicalDataSource({
                transport: {
                    read: {
                        url: dataUrl,
                        dataType: "json"
                    }
                },
                schema: {
                    model: {
                        id: "Id",
                        hasChildren: "HasChildren",
                        children: "Children",
                    }
                },
				//sort by
				sort: { field: "Name", dir: "asc" }
            });
			//Show Modal After DataSourse Ready
			$('#myModal').modal('show');
			//Run Kendo Treeviwe
			$("#treeview").kendoTreeView({
				dataSource: dataSource,
				dataTextField: "Name",
				select: _onSelect
			});
		}
		
		this.showTreeObjectPicker = _showTreeObjectPicker;
}