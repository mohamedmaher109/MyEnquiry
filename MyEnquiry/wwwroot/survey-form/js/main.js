$(document).on("keyup", ".form-control", function () {
	var maxCount = $(this).data('max-count');
	var id = $(this).attr('id');
	if (maxCount != null && maxCount != NaN && maxCount != undefined && maxCount != "") {
		var characterCount = $(this).val().length,
			current = $('#current_' + id),
			maximum = $('#maximum_' + id),
			theCount = $('#the-count_' + id);

		if (characterCount > maxCount) {
			var val = $(this).val().substr(0, $(this).val().length - 1);
			$(this).val(val);
			characterCount = maxCount;
		}

		current.text(characterCount);

		if (characterCount >= maxCount) {
			maximum.css('color', '#8f0001');
			current.css('color', '#8f0001');
			theCount.css('font-weight', 'bold');
		} else {
			maximum.css('color', 'rgb(102, 102, 102)');
			current.css('color', 'rgb(102, 102, 102)');
			theCount.css('font-weight', 'normal');
		}

		var currentValueLength = $(this).val().length;
		if (currentValueLength > maxCount) {
			characterCount = maxCount;
			var val = $(this).val().substr(0, maxCount);
			$(this).val(val);
			current.text(characterCount);
		}
	}
});

$(document).ready(function () {
	$("#formAdd").ajaxForm({
		beforeSubmit: function (e) {
			$(".submit-btn").attr('disabled', 'true');
		}
		,
		success: function (res, status, xhr, form) {
			swal({
				title: "الرد على الإستعلام",
				text: res.msg,
				type: "success"
			});
			$('#formAdd').resetForm();
			$(".submit-btn").removeAttr('disabled');
			var current = $('span[id^="current"]');
			var maximum = $('span[id^="maximum"]');
			var theCount = $('div[id^="the-count"]');

			current.each(function (e) {
				$(this).css('color', 'rgb(102, 102, 102)');
				$(this).text('0');
			})

			maximum.each(function (e) {
				$(this).css('color', 'rgb(102, 102, 102)');
			})

			theCount.each(function (e) {
				$(this).css('font-weight', 'normal');
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
			$(".submit-btn").removeAttr('disabled');
		}
	})
})