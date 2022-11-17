
var selectedElmsIds = [];





















$(document).on("change", ".RoleId", function () {

	debugger
	var id = $(this).val();
	if (id == null || id == "") {
		swal({
			title: "الصلاحيات",
			text: "يجب اختيار المجموعه لاضافة الصلاحيات",
			type: "warning"
		});
		$('.displaytree').jstree("destroy").empty();
		return false;
    }

	$(".roleid").val(id);
	$.ajax({
		async: true,
		type: "GET",
		url: "/User/Permissions",
		data: { 'Id': id },
		dataType: "json",
		success: function (json) {
			$('.displaytree').jstree("destroy").empty()
			/*$('.inmodal').appendTo("body")*/
			createJSTree(json);
		},

		error: function (xhr, ajaxOptions, thrownError) {
			alert(xhr.status);
			alert(thrownError);
		}
	});





});
function createJSTree(jsondata) {
	$('.displaytree').jstree({
		'core': {
			'data': jsondata
		}
		, "checkbox": {
			"keep_selected_style": false
		},

		"plugins": ["checkbox"]
	});
}

function submitMe() {
	selectedElmsIds = [];
	var selectedElms = $('.displaytree').jstree("get_selected", true);
	$.each(selectedElms, function () {
		selectedElmsIds.push(this.id);
	});


	var roleid = $(".RoleId").val();

	if (roleid == null || roleid == undefined || roleid === "") {
		swal({
			title: "الصلاحيات",
			text: "يجب اختيار المجموعه المراد اضافة صلاحية لها",
			type: "warning"
		});
		return false;
	}

	$.ajax({
		async: true,
		type: "POST",
		url: "/User/PostPermissions",
		dataType: "json",
		data: { "Ids": selectedElmsIds, "roleid": roleid },
		success: function (res) {
			swal({
				title: "الصلاحيات",
				text: res.msg,
				type: "success"
			});
			$('.displaytree').jstree(true).refresh();
			$('.inmodal').modal('hide');
			window.location.reload();
		},

		error: function (res) {
			try {
				swal({
					title: "خطأ",
					text: res.responseJSON.message,
					type: "error"
				});
			} catch (e) {
				swal({
					title: "خطأ",
					text: "برجاء المحاولة مرة اخرى",
					type: "error"
				});
			}
		}
	});


};
