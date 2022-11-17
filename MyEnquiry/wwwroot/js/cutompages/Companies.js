






$(document).ready(function () {




	$("#formadd").ajaxForm({


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
					title: "الشركات",
					text: "يجب ادخال اسم الشركة",
					type: "warning"
				});
				return false;
			}




		}
		,

		success: function (res, status, xhr, form) {
			swal({
				title: "الشركات",
				text: res.msg,
				type: "success"
			});
			$('#formadd').resetForm();

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
		url: "/Company/DisplayGrid",
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








$(document).on("click", ".editcompany", function () {

	var id = $(this).data('id');
	$.ajax({
		url: "/Company/GetCompany",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disedit").html(html);
			$('#EditCompanyModal').modal('show');


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
							title: "تعديل الشركة",
							text: "يجب ادخال اسم الشركة",
							type: "warning"
						});
						return false;
					}




				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "تعديل الشركة",
						text: res.msg,
						type: "success"
					});
					$('#formaEdit').resetForm();
					$('#EditCompanyModal').modal('hide');
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





$(document).on("click", ".deletecompany", function () {
	debugger
	var id = $(this).data('id');



	swal({
		title: "هل انت متأكد?",
		text: "لن تستطيع استعادة هذه الشركة مره اخرى",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم احذفها !",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/Company/Delete",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "حذف الشركة",
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
