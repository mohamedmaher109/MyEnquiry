






$(document).ready(function () {




	$("#formaddbank").ajaxForm({


		beforeSubmit: function (e) {
			//if (document.getElementsByName("Logofile").files.length == 0) {
			//	swal({
			//		title: "اللوجو مطلوب",
			//		text: "يجب اختيار لوجو البنك",
			//		type: "warning"
			//	});
			//	return false;
			//}



			var isValid = true;
			$(".req").each(function () {
				var element = $(this);
				if (element.val() == "") {

					isValid = false;
				}

			});

			if (isValid == false) {

				swal({
					title: "البنوك",
					text: "يجب ادخال اسم البنك",
					type: "warning"
				});
				return false;
			}




		}
		,

		success: function (res, status, xhr, form) {
			swal({
				title: "البنوك",
				text: res.msg,
				type: "success"
			});
			$('#formaddbank').resetForm();

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


	refreshgrid();


})















function refreshgrid() {
	$.ajax({
		url: "/Banks/DisplayGrid",
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








$(document).on("click", ".editbank", function () {

	var id = $(this).data('id');
	$.ajax({
		url: "/Banks/Getbank",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disedit").html(html);
			$('#EditBankModal').modal('show');


			$("#formaEdit").ajaxForm({


				beforeSubmit: function (e) {



					debugger
					var isValid = true;
					$(".reqE").each(function () {
						var element = $(this);
						if (element.val() == "") {

							isValid = false;
						}

					});

					if (isValid == false) {

						swal({
							title: "تعديل البنك",
							text: "يجب ادخال اسم البنك",
							type: "warning"
						});
						return false;
					}




				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "تعديل البنك",
						text: res.msg,
						type: "success"
					});
					$('#formaEdit').resetForm();
					$('#EditBankModal').modal('hide');
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
		}
	});


})





$(document).on("click", ".deletebank", function () {
	debugger
	var id = $(this).data('id');



	swal({
		title: "هل انت متأكد?",
		text: "لن تستطيع استعادة هذا البنك مره اخرى",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم احذفها !",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/Banks/Delete",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "حذف البنك",
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
