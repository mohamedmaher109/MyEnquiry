$(document).ready(function () {







	$("#LoginForm").ajaxForm({


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
					title: "تسجيل الدخول",
					text: "يجب ادخال البريد الالكترونى وكلمة المرور",
					type: "warning"
				});

				return false;
			}




		}
		,
		success: function (res) {
			try {
				var base_url = window.location.origin;
				window.location.href = base_url + res.rout;
			} catch (e) {
				swal({
					title: "خطأ",
					text: "برجاء اعادة المحاولة مره اخرى",
					type: "error"
				});
			}
			$('body').removeClass('loading');

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
					text: "برجاء اعادة المحاولة مره اخرى",
					type: "error"
				});
			}
			$('body').removeClass('loading');

		}
	})





})