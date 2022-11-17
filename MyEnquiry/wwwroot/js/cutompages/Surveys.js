$(document).ready(function () {
	$("#formadd").ajaxForm({
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
					title: "الإستعلامات",
					text: "يجب ادخال اسم الإستعلام",
					type: "warning"
				});
				return false;
			}
		}
		,

		success: function (res, status, xhr, form) {
			swal({
				title: "الإستعلامات",
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
		url: "/Surveys/DisplayGrid",
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

$(document).on("click", ".editregion", function () {

	var id = $(this).data('id');
	$.ajax({
		url: "/Surveys/GetSurvey",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disedit").html(html);
			$('#EditSurveyModal').modal('show');
			$("#formaEdit").ajaxForm({
				beforeSubmit: function (e) {
					var isValid = true;
					$(".reqE").each(function () {
						var element = $(this);
						if (element.val() == "") {
							isValid = false;
						}
					});

					if (isValid == false) {
						swal({
							title: "تعديل الإستعلام",
							text: "يجب ادخال اسم الإستعلام",
							type: "warning"
						});
						return false;
					}
				}
				,
				success: function (res, status, xhr, form) {
					swal({
						title: "تعديل الإستعلام",
						text: res.msg,
						type: "success"
					});
					$('#formaEdit').resetForm();
					$('#EditSurveyModal').modal('hide');
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

$(document).on("click", ".deleteregion", function () {
	var id = $(this).data('id');
	swal({
		title: "هل انت متأكد?",
		text: "لن تستطيع استعادة هذه الإستمارة مره اخرى",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم احذفها !",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/Surveys/Delete",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "حذف الإستعلام",
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

$(document).on("click", ".copy-form-url", function () {
	var formIdentifier = $(this).data('form-identifier');
	var url = window.location.origin + '/Surveys/Form?formId=' + formIdentifier;
	navigator.clipboard.writeText(url);
	swal({
		title: "تم بنجاح",
		text: 'تم نسخ رابط استمارة الإستعلام',
		type: "success"
	});
})


$(document).on("click", ".open-link", function () {
	var formIdentifier = $(this).data('form-identifier');
	var url = window.location.origin + '/Surveys/Form?formId=' + formIdentifier;
	window.open(url);
})


$(document).on("click", ".CopySurvey", function () {
	var id = $(this).data('id');
	swal({
		title: "هل انت متأكد من نسخ الحاله?",
		text: "",
		type: "success",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم إنسخها !",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/Surveys/CopySurvey",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "نسخ الإستعلام",
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