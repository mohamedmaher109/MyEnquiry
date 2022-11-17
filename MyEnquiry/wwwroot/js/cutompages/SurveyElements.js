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

