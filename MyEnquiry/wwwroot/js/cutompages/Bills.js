$(document).ready(function () {



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



	$("#customRadio1").trigger("change");


})




/*function refreshgrid() {
	var id = $(this).data('ids');
	$.ajax({
		url: "/Bills/DisplayGrid2",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".dispaly54").html(html);
			$('#dataTable-3').DataTable(
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
*/
$(document).ready(function () {



		var id = $('#Vale').val();

		$.ajax({
			url: "/Bills/DisplayGrid2",
			cache: false,
			type:'Get',
			data: { Id: id },
			success: function (html) {
				$(".dispaly54").html(html);
				$('#dataTable-3').DataTable(
					{
						autoWidth: true,
						"lengthMenu": [
							[10, 20, 40, -1],
							[10, 20, 40, "All"]
						]
					});
			}

		})
	
})
$(document).ready(function () {
	$.ajax({
		url: "/Bills/DisplayGrid",
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
})

$('#formaddUser input[name=Usertype]').on('change', function () {
	var value = $('input[name=Usertype]:checked', '#formaddUser').val();
	debugger;
	switch (value) {
		
		case "1":
			
				$('.companycol').attr('hidden', true);
			$('.cashnum').attr('hidden', true);
			
			
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















$(document).on("click", ".TotalComp", function () {
	debugger
	var id = $(this).data('id');




		$.ajax({
			url: "/Dues/GetTotalCompany",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "إجمالى المبلغ لهذه الشركه",
					text: res.msg,
					type: "success"
				});
				
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

