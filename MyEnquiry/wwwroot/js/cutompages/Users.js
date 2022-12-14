$(document).ready(function () {


	refreshCashgrid();

	$("#formaddUser").ajaxForm({


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
					title: "اضافة مستخدم",
					text: "يجب كتابة كل البيانات المطلوبة",
					type: "warning"
				});
				return false;
			}




		}
		,

		success: function (res, status, xhr, form) {
			swal({
				title: "اضافة مستخدم",
				text: res.msg,
				type: "success"
			});
			$('#formaddUser').resetForm();


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
					text: "Something Wrong Please Try Again",
					type: "error"
				});
			}

		}
	})



	refreshgrid();
	$("#customRadio1").trigger("change");


})




function refreshgrid() {
	$.ajax({
		url: "/User/DisplayUserGrid",
		cache: false,
		success: function (html) {
			$(".dispalygrid").html(html);
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



function refreshCashgrid() {
	$.ajax({
		url: "/User/DisplayCashGrid",
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
		}

	});


}


$('#formaddUser input[name=Usertype]').on('change', function () {
	var value = $('input[name=Usertype]:checked', '#formaddUser').val();

	switch (value) {
		case "1":
		    $(".companycol").attr('hidden', true);
			$(".cashnum").attr('hidden', true);
			$(".bankcol").removeAttr("hidden");
			$(".bankreq").addClass("req");
			$(".companyreq").removeClass("req");
			$(".cashp").removeClass("req");
	
			break;
		case "2":
		
				$(".bankcol").attr('hidden', true);
			$(".bankreq").removeClass("req");
			
			$(".companycol").removeAttr("hidden");
			$(".cashnum").removeAttr("hidden");
			$(".companyreq").addClass("req");
			$(".cashp").addClass("req");
		

			break;
		case "3":
			
				$(".bankcol").attr('hidden', true);
			
				$(".companycol").attr('hidden', true);
			$(".cashnum").removeClass("req");
			$(".bankreq").removeClass("req");
			$(".companyreq").removeClass("req");
			$(".cashp").removeClass("req");
		
			
			break;
		case "5":
				$(".bankcol").attr('hidden', true);
			
				$(".companycol").attr('hidden', true);
			$(".cashnum").removeClass("req");
			$(".bankreq").removeClass("req");
			$(".companyreq").removeClass("req");
			$(".cashp").removeClass("req");
	
			break;
        default:
    }
});


function myFunction() {
	debugger
	var x = document.getElementById("Password");
	var y = document.getElementById("confirmpassword");
	if (x.type === "password") {
		x.type = "text";
	} else {
		x.type = "password";
	}
	if (y.type === "password") {
		y.type = "text";
	} else {
		y.type = "password";
	}
}
$(document).on("click", ".changepass", function () {
	debugger
	var id = $(this).data('id');
	$.ajax({
		url: "/User/Changepassword",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disedit").html(html);
			$('#ChangePasswordUserModal').modal('show');
			$("#formaChangepasswordUser").ajaxForm({
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
							title: "تعديل المستخدم",
							text: "كل الحقول مطلوبة",
							type: "warning"
						});
						return false;
					}




				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "تعديل المستخدم",
						text: res.msg,
						type: "success"
					});
					$('#formaChangepasswordUser').resetForm();
					$('#ChangePasswordUserModal').modal('hide');
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
							text: "برجاء المحاولة في وقت لاحق",
							type: "error"
						});
					}

				}
			})
		}
	});


})











$(document).on("click", ".edituser", function () {

	var id = $(this).data('id');
	$.ajax({
		url: "/User/GetUser",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disedit").html(html);
			$('#EditUserModal').modal('show');
			$("#formaEditUser").ajaxForm({
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
							title: "تعديل المستخدم",
							text: "كل الحقول مطلوبة",
							type: "warning"
						});
						return false;
					}




				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "تعديل المستخدم",
						text: res.msg,
						type: "success"
					});
					$('#formaEditUser').resetForm();
					$('#EditUserModal').modal('hide');
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
							text: "برجاء المحاولة في وقت لاحق",
							type: "error"
						});
					}

				}
			})
		}
	});


})















$(document).on("click", ".deleteuser", function () {
	debugger
	var id = $(this).data('id');



	swal({
		title: "هل انت متاكد?",
		text: "لن تستطيع استعادة هذا المتخدم!",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم احذف على اى حال",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/User/DeleteUser",
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

