var isblockindex;


var lat;
var lng;

$(document).ready(function () {




	$("#formadd").ajaxForm({


		beforeSubmit: function (e) {




			validateForm();



		}
		,

		success: function (res, status, xhr, form) {
			swal({
				title: "تعديل مندوب",
				text: res.msg,
				type: "success"
			});
			$('#formadd').resetForm();
			var base_url = window.location.origin;
			window.location.href = base_url + res.rout;

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

	$("#formaedit").ajaxForm({


		beforeSubmit: function (e) {




			validateFormEdit();



		}
		,

		success: function (res, status, xhr, form) {
			swal({
				title: "تعديل مندوب",
				text: res.msg,
				type: "success"
			});
			
			var base_url = window.location.origin;
			window.location.href = base_url + res.rout;

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


	if (isblockindex == true) {
		refreshBlockgrid();
	}
	else {
		refreshgrid();
    }
	


})



//function addEventHandler(node, evt, func) {
//	if (node.addEventListener)
//		node.addEventListener(evt, func);
//	else
//		node.attachEvent("on" + evt, func);
//}

//function initializeAll() {
//	addEventHandler(document.getElementById('formadd'), 'submit', validateForm);
//}
//addEventHandler(window, 'load', initializeAll);



function validateForm() {

	
	var args = Array.prototype.slice.call(arguments),
		e = args[0];
	var isValid = true;
	$(".req").each(function () {
		var element = $(this);
		if (element.val() == "") {

			isValid = false;
		}

	});

	if (isValid == false) {

		swal({
			title: "اضافة مندوب",
			text: "يجب كتابة كل البيانات المطلوبة",
			type: "warning"
		});
		e.cancelBubble = true;
		e.returnValue = false;
		if (e.preventDefault) {
			e.preventDefault();
		}
		return false;
	}



	if (document.getElementById("NationalMediaFront").files.length == 0) {
		swal({
			title: "صورة البطاقة",
			text: "يجب اختيار صورة البطاقة الامامية",
			type: "warning"
		});
		e.cancelBubble = true;
		e.returnValue = false;
		if (e.preventDefault) {
			e.preventDefault();
		}
		return false;
	}
	
	if (document.getElementById("NationalMediaBack").files.length == 0) {
		swal({
			title: "صورة البطاقة",
			text: "يجب اختيار صورة البطاقة الخلفية",
			type: "warning"
		});
		e.cancelBubble = true;
		e.returnValue = false;
		if (e.preventDefault) {
			e.preventDefault();
		}
		return false;
	}
	
	if (document.getElementById("CriminalFish").files.length == 0) {
		swal({
			title: "الفيش الجنائى",
			text: "يجب اختيار الفيش الجنائى",
			type: "warning"
		});
		e.cancelBubble = true;
		e.returnValue = false;
		if (e.preventDefault) {
			e.preventDefault();
		}
		return false;
	}
	
	if (document.getElementById("AcadimicQualification").files.length == 0) {
		swal({
			title: "المؤهل",
			text: "يجب اختيار صورة المؤهل",
			type: "warning"
		});
		e.cancelBubble = true;
		e.returnValue = false;
		if (e.preventDefault) {
			e.preventDefault();
		}
		return false;
	}




}





function validateFormEdit() {

	
	var args = Array.prototype.slice.call(arguments),
		e = args[0];
	var isValid = true;
	$(".req").each(function () {
		var element = $(this);
		if (element.val() == "") {

			isValid = false;
		}

	});

	if (isValid == false) {

		swal({
			title: "تعديل مندوب",
			text: "يجب كتابة كل البيانات المطلوبة",
			type: "warning"
		});
		e.cancelBubble = true;
		e.returnValue = false;
		if (e.preventDefault) {
			e.preventDefault();
		}
		return false;
	}





}













function refreshBlockgrid() {
	$.ajax({
		url: "/Representative/DisplayBlockedGrid",
		cache: false,
		success: function (html) {
			$(".dispalyblockedgrid").html(html);
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






















$(document).on("click", ".delete", function () {
	debugger
	var id = $(this).data('id');



	swal({
		title: "هل انت متاكد?",
		text: "لن تستطيع استعادة هذا المندوب!",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم احذف على اى حال",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/Representative/Delete",
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





$(document).on("click", ".block", function () {
	debugger
	var id = $(this).data('id');
	var type = $(this).data('type');
	var txt = "";
	if (type == 1) {
		txt = "هل انت متاكد انك تريد حظر هذا المندوب"
	}
	else {
		txt="هل انت متاكد انك تريد الغاء حظر هذا المندوب"
    }

	swal({
		title: "هل انت متاكد?",
		text: txt,
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "نعم  على اى حال",
		closeOnConfirm: false
	}, function () {
		$.ajax({
			url: "/Representative/Block",
			cache: false,
			data: { 'Id': id },
			success: function (res) {
				swal({
					title: "المندوب",
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


$(document).on("click", ".location", function () {


	var id = $(this).data('id');
	$.ajax({
		url: "/Representative/Userlocation",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			lat = $("#userlat").val();
			lng = $("#userlng").val();
			debugger
			$('#LocationModal').modal('show');
			initMap(lat,lng);

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



function refreshgrid() {
	$.ajax({
		url: "/Representative/DisplayGrid",
		cache: false,
		success: function (html) {
			$(".dispalygridTY").html(html);
			$('#dataTable-28').DataTable(
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

// Initialize and add the map
function initMap( lat,  lng) {
	debugger
	// The location of Uluru
	const uluru = { lat: parseFloat(lat), lng: parseFloat(lng) };
	// The map, centered at Uluru
	const map = new google.maps.Map(document.getElementById("map"), {
		zoom: 15,
		center: uluru,
	});
	// The marker, positioned at Uluru
	const marker = new google.maps.Marker({
		position: uluru,
		map: map,
	});
}


$(document).on("click", ".showdata", function () {


	var id = $(this).data('id');
	$.ajax({
		url: "/Representative/GetFiles",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			$('#FilesModal').modal('show');


		}
	});



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
/*
$(document).on("click", ".changepassss", function () {


	var id = $(this).data('id');
	debugger
	$.ajax({
		url: "/Representative/Changepassword",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			$('#ChangePasswordModal').modal('show');


		}
	});



});*/
    
// change bassword

$(document).on("click", ".changepassss", function () {
		debugger
	var id = $(this).data('id');
	$.ajax({
		url: "/Representative/Changepassword",
		cache: false,
		data: { 'Id': id },
		success: function (html) {
			$(".disfiles").html(html);
			$('#ChangePasswordModal').modal('show');
			$("#formaChangepassword").ajaxForm({
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
					$('#ChangePasswordModal').resetForm();
					$('#ChangePasswordModal').modal('hide');
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
