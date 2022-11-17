






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
					title: "إضافة الأسعار",
					text: "يجب ادخال هذا الحقل ",
					type: "warning"
				});
				return false;
			}




		}
		,

		success: function (res, status, xhr, form) {
			swal({
				title: "إضافة الأسعار",
				text: res.msg,
				type: "success"
			});
			$('#formaddbank').resetForm();

			refreshgrid1();
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


	
})



$(document).ready(function () {
	refreshgrid();

	refreshgrid1();


})











function refreshgrid() {
	$.ajax({
		url: "/BankCompanyCase/DisplayGrid",
		cache: false,
		success: function (html) {
			$(".dispalygrid3").html(html);

			$('#dataTable-2').DataTable(
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


function refreshgrid1() {
	var id2 = $('#kjdk').val();
	$.ajax({
		url: "/BankCompanyCase/DisplayGridBanc",
		cache: false,
		data: { 'id': id2 },
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
		url: "/BankCompanyCase/GetbankCase",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disedit").html(html);
			$('#EditBankCaseModal').modal('show');


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
						title: "تعديل سعر الحاله",
						text: res.msg,
						type: "success"
					});
					$('#formaEdit').resetForm();
					$('#EditBankModal').modal('hide');
					refreshgrid();
					refreshgrid1();
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





$(document).on("click", ".delete", function () {
	debugger
	var id = $(this).data('id');



	swal({
		title: "هل انت متأكد?",
		text: "لن تستطيع استعادة هذا  مره اخرى",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم احذفها !",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/BankCompanyCase/Delete",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "حذف البنك",
					text: res.msg,
					type: "success"
				});
				refreshgrid();
				refreshgrid1();
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
