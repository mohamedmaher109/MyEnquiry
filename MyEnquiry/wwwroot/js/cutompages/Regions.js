






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
					title: "المناطق",
					text: "يجب ادخال اسم المنطقة",
					type: "warning"
				});
				return false;
			}




		}
		,

		success: function (res, status, xhr, form) {
			swal({
				title: "المناطق",
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
		url: "/Region/DisplayGrid",
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
		url: "/Region/GetRegion",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disedit").html(html);
			$('#EditRegionModal').modal('show');


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
							title: "تعديل المنطقة",
							text: "يجب ادخال اسم المنطقة",
							type: "warning"
						});
						return false;
					}




				}
				,

				success: function (res, status, xhr, form) {
					swal({
						title: "تعديل المنطقة",
						text: res.msg,
						type: "success"
					});
					$('#formaEdit').resetForm();
					$('#EditRegionModal').modal('hide');
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
	debugger
	var id = $(this).data('id');



	swal({
		title: "هل انت متأكد?",
		text: "لن تستطيع استعادة هذه المنطقة مره اخرى",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم احذفها !",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/Region/Delete",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "حذف المنطقة",
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
