






$(document).ready(function () {



	refreshgrid();


})















function refreshgrid() {
	$.ajax({
		url: "/RefusedCases/Display",
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






;


$(document).on("click", ".solve", function () {

	debugger
	var id = $(this).data("id");
	
	$.ajax({
		url: "/RefusedCases/Solved",
		cache: false,
		data: { "Id": id },
		success: function (res) {
			swal({
				title: "الحالات",
				text: res.msg,
				type: "success"
			});
			

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
	});


});

