






$(document).ready(function () {



	refreshgrid();


})















function refreshgrid() {
	$.ajax({
		url: "/DoneCases/Display",
		cache: false,
		
		success: function (html) {
			$(".dispaly").html(html);

			$('#dataTable-1').DataTable(
				{
					autoWidth: true,
					"lengthMenu": [
						[10, 20, 40, -1],
						[10, 20, 40, "All"]
					]
				});

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
}









$(document).on("click", ".accept", function () {

	debugger
	var id = $(this).data("id");
	var type = $(this).data("type");
	$.ajax({
		url: "/DoneCases/ChangeStatus",
		cache: false,
		data: { "Id": id, "type": type },
		success: function (res) {
			swal({
				title: "الحالات",
				text: res.msg,
				type: "success"
			});


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
	});


});





$(document).on("click", ".reject", function () {

	var id = $(this).data('id');

	$.ajax({
		url: "/DoneCases/OpenRejectModal",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			$('#SendCaseModal').modal('show');


			$("#formsend").ajaxForm({


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
							title: "الحالة",
							text: "يجب كتابة سبب الرفض",
							type: "warning"
						});
						return false;
					}





				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "الحالة",
						text: res.msg,
						type: "success"
					});
					$('#formsend').resetForm();
					$('#SendCaseModal').modal('hide');
					refreshCasesgrid();

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
		}
	});


})


