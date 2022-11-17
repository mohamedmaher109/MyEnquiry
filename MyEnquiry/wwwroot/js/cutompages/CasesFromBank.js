






$(document).ready(function () {



	//refreshCasesFromBankgrid();


})















function refreshCasesFromBankgrid(id) {
	$.ajax({
		url: "/Cases/DisplayCasesFromBank",
		cache: false,
		data: {"BankId":id},
		success: function (html) {
			$(".dispalyCasesFromBank").html(html);

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







$(document).on("change", ".BankId", function () {

	debugger
	var id = $(this).val();
	if (id == null || id == "") {
		swal({
			title: "البنوك",
			text: "يجب اختيار البنك لظهار الحالات القادمة منها",
			type: "warning"
		});
		return false;
	}

	refreshCasesFromBankgrid(id);



});


$(document).on("click", ".recive", function () {

	debugger
	var id = $(this).data("id");
	var type = $(this).data("type");
	var bankid = $(this).data("bankid");
	$.ajax({
		url: "/Cases/Recive",
		cache: false,
		data: { "Id": id, "type": type },
		success: function (res) {
			swal({
				title: "الحالات",
				text: res.msg,
				type: "success"
			});
			

			refreshCasesFromBankgrid(bankid);

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



$(document).on("click", ".upload", function () {

	var id = $(this).data('id');
	var bankid = $(this).data("bankid");

	$.ajax({
		url: "/Cases/OpenUploadModal",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			$('#UploadModal').modal('show');


			$("#formaupload").ajaxForm({


				beforeSubmit: function (e) {

					if (document.getElementById("excelfile").files.length == 0) {
						swal({
							title: "شيت الاكسيل",
							text: "يجب اختيار شيت الاكسيل",
							type: "warning"
						});
						return false;
					}

					




				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "شيت الاكسيل",
						text: res.msg,
						type: "success"
					});
					$('#formaupload').resetForm();
					$('#UploadModal').modal('hide');
					refreshCasesFromBankgrid(bankid);

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



$(document).on("click", ".deleteAllCases", function () {
	debugger
	var id = $(this).data('id');



	swal({
		title: "هل انت متاكد?",
		text: "لن تستطيع استعادة هذه الحالات مره أخرى!",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم احذف على اى حال",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/Cases/deleteAllCases",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "حذف",
					text: res.msg,
					type: "success"
				});
				refreshgrid();
			}
			, error: function (res) {
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




});


$(document).on("click", ".refusedcases", function () {

	

	$.ajax({
		url: "/Cases/OpenRefuseModal",
		cache: false,
		
		success: function (html) {
			$(".disfiles").html(html);
			$('#RefusedModal').modal('show');


			//$("#formRefused").ajaxForm({


			//	beforeSubmit: function (e) {
			//if ($("#bankrefuse").val() == null || $("#bankrefuse").val() == "") {
			//	swal({
			//				title: "البنك",
			//				text: "يجب اختيار البنك المراد ارسال له الحالات المرفوضة",
			//				type: "warning"
			//			});
			//			return false;
   //         }
			//		if (document.getElementById("excelfile").files.length == 0) {
			//			swal({
			//				title: "شيت الاكسيل",
			//				text: "يجب اختيار شيت الاكسيل",
			//				type: "warning"
			//			});
			//			return false;
			//		}






			//	}
			//	,

			//	success: function (res, status, xhr, form) {
			//		swal({
			//			title: "شيت الاكسيل",
			//			text: res.msg,
			//			type: "success"
			//		});
			//		$('#formRefused').resetForm();
			//		$('#RefuseModal').modal('hide');

			//	},
			//	error: function (res) {
			//		try {
			//			swal({
			//				title: "خطأ",
			//				text: res.responseJSON.message,
			//				type: "error"
			//			});
			//		} catch (e) {
			//			swal({
			//				title: "خطأ",
			//				text: "برجاء المحاولة مرة اخرى",
			//				type: "error"
			//			});
			//		}

			//	}
			//})
		}
	});


})

