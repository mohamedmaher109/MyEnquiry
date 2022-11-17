var myAppendGrid;



$(document).ready(function () {

    InitAppendGrid();


	refreshgrid();



	$("#formadd").ajaxForm({
		beforeSerialize: function ($form, options) {
			var data = myAppendGrid.getAllValue();

			var strdata = JSON.stringify(data);

			$("#Form").val(strdata).trigger("change");
		},

		beforeSubmit: function (e) {
			

			var isValid = true;
			$(".req").each(function () {
				var element = $(this);
				if (element.val() == "") {

					isValid = false;
				}

			});

			if (isValid == false) {

				swal({
					title: "الاستمارات",
					text: "يجب ادخال اختيار نوع الحالة",
					type: "warning"
				});
				return false;
			}




		}
		,

		success: function (res, status, xhr, form) {
			swal({
				title: "استمارة",
				text: res.msg,
				type: "success"
			});
			$('#formadd').resetForm();
			InitAppendGrid();
			refreshgrid();

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
	})

});
function InitAppendGrid() {
     myAppendGrid = new AppendGrid({
        element: "tblAppendGrid",
        uiFramework: "bootstrap5",
        iconFramework: "bootstrapicons",
        iconParams: {
            baseUrl: "https://cdn.jsdelivr.net/npm/bootstrap-icons/icons/"
        },
        columns: [
            {
				name: "question",
                display: "اكتب السؤال",
                ctrlAttr: { required: "required" },

            },
			{
				name: "answer",
				display: "اكتب الاجابة اذا كان هناك اكثر من اجابة ضع بين الاجابات علامه - واذا كنت تريد ان يجاوب عليها المستخدم اتركها فارغة",
			
				ctrlAttr: {  placeholder:"مثال :اجابة1-اجابة2-اجابة3"},

			},
            {
				name: "hasfile",
                displayClass: "text-center",
                display: "الاجابة بصوره؟",
                type: "checkbox",
                cellClass: "text-center",

            },


        ],
        // Optional CSS classes, to make table slimmer!
        sectionClasses: {
            table: "table-sm",
            control: "form-control-sm",
            buttonGroup: "btn-group-sm"
        }
    });


}





function refreshgrid() {
	$.ajax({
		url: "/CaseForms/DisplayGrid",
		cache: false,
		success: function (html) {
			$(".dispalygrid").html(html);

			$('#dataTable-1').DataTable(
				{
					autoWidth: true,
					"lengthMenu": [
						[10, 20, 40, -1],
						[10, 20, 40, "All"]
					]
				});

		}
	});
}




$(document).on("click", ".delete", function () {
	debugger
	var id = $(this).data('id');



	swal({
		title: "هل انت متأكد?",
		text: "لن تستطيع استعادة هذه الاستمارة مره اخرى",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم احذفها !",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/CaseForms/Delete",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "حذف الاستمارة",
					text: res.msg,
					type: "success"
				});
				refreshgrid();
			}
			, error: function (res) {
				try {
					swal({
						title: "Error",
						text: res.responseJSON.message,
						type: "error"
					});
				} catch (e) {
					swal({
						title: "خطا",
						text: "برجاء المحاولة مرة اخرى",
						type: "error"
					});
				}
			}
		});
	});




});
