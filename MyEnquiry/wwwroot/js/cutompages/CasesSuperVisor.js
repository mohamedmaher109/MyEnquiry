﻿






$(document).ready(function () {



	refreshCasesgrid();


})















function refreshCasesgrid(id) {
	$.ajax({
		url: "/SuperVisorCases/DisplayCases",
		cache: false,
		data: {"Status":id},
		success: function (html) {
			$(".dispalyCases").html(html);

			$('#dataTable-1').DataTable(
				{
					//autoWidth: false,
					//"lengthMenu": [
					//	[10, 20, 40, -1],
					//	[10, 20, 40, "All"],
						
					//],
					//columnDefs: [
					//	{ width: 100, targets: 0 },
					//	{ width: 100, targets: 1 },
					//	{ width: 100, targets: 2 },
					//	{ width: 100, targets: 3 },
					//	{ width: 100, targets: 4 }
					//],
					//fixedColumns: true
					destroy: true,
					"processing": true, // for show progress bar    
					"serverSide": false, // for process server side    
					"filter": true, // this is for disable filter (search box)    
					"orderMulti": false, // for disable multiple column at once   
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







$(document).on("change", ".StatusId", function () {

	debugger
	var id = $(this).val();
	

	refreshCasesgrid(id);



});





$(document).on("click", ".sendcase", function () {

	var id = $(this).data('id');

	$.ajax({
		url: "/SuperVisorCases/OpenRepresentativeModal",
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
							title: "المندوب",
							text: "يجب اختيار المندوب",
							type: "warning"
						});
						return false;
					}





				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "مندوب",
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






$(document).on("click", ".acceptcase", function () {
	debugger
	var id = $(this).data('id');



	swal({
		title: "هل انت متأكد?",
		text: "فى حالة قبول الحالة سيتم ارسالها الى الجزدة",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#66bf56",
		confirmButtonText: "نعم اقبل !",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/SuperVisorCases/AcceptCase",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "قبول الحالة",
					text: res.msg,
					type: "success"
				});
				refreshCasesgrid(id);

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







$(document).on("click", ".uploadimg", function () {

	var id = $(this).data('id');

	$.ajax({
		url: "/Cases/OpenUploadModal",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			$('#UploadModal').modal('show');


			$("#formaupload").ajaxForm({


				beforeSubmit: function (e) {

					if (document.getElementById("file").files.length == 0) {
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








$(document).on("click", ".rate", function () {

	var id = $(this).data('id');

	$.ajax({
		url: "/SuperVisorCases/RateModal",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			$('#RateModal').modal('show');


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
							title: "مطلوب",
							text: "يجب ادخال البيانات المطلوبة",
							type: "warning"
						});
						return false;
					}





				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "المراجع",
						text: res.msg,
						type: "success"
					});
					$('#formsend').resetForm();
					$('#RateModal').modal('hide');

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







$(document).on("click", ".filesFromRep", function () {


	var id = $(this).data('id');
	var type = $(this).data('type');
	$.ajax({
		url: "/SuperVisorCases/GetFiles",
		cache: false,
		data: { 'Id': id, 'type': type },
		success: function (html) {
			$(".disfiles").html(html);
			$('#FilesModal').modal('show');


		}
	});



});


$(document).on("click", ".reviewcase", function () {


	var id = $(this).data('id');
	$.ajax({
		url: "/CasesTracking/ReviewCaseModal",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			$('#reviewModal').modal('show');

			$("#formEditCaseForm").ajaxForm({


				beforeSubmit: function (e) {




					//var isValid = true;
					//$(".req").each(function () {
					//	var element = $(this);
					//	if (element.val() == "") {

					//		isValid = false;
					//	}

					//});

					//if (isValid == false) {

					//	swal({
					//		title: "مطلوب",
					//		text: "يجب ادخال البيانات المطلوبة",
					//		type: "warning"
					//	});
					//	return false;
					//}





				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "مراجعة الحالة",
						text: res.msg,
						type: "success"
					});
					$('#formEditCaseForm').resetForm();
					$('#reviewModal').modal('hide');

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





$(document).on("click", ".showanswerfile", function () {


	var id = $(this).data('id');
	$.ajax({
		url: "/CasesTracking/GetFormFile",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".modalfileform").html(html);
			$('#FilesModal').modal('show');


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

$(document).on("click", ".UploadfileToBank", function () {


	var id = $(this).data('id');
	$.ajax({
		url: "/CasesTracking/UploadAnswerForBank",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			$('#reviewModalBank').modal('show');

			$("#formEditBankForm").ajaxForm({


				beforeSubmit: function (e) {




					//var isValid = true;
					//$(".req").each(function () {
					//	var element = $(this);
					//	if (element.val() == "") {

					//		isValid = false;
					//	}

					//});

					//if (isValid == false) {

					//	swal({
					//		title: "مطلوب",
					//		text: "يجب ادخال البيانات المطلوبة",
					//		type: "warning"
					//	});
					//	return false;
					//}





				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "مراجعة الحالة",
						text: res.msg,
						type: "success"
					});
					$('#formEditCaseForm').resetForm();
					$('#reviewModal').modal('hide');

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