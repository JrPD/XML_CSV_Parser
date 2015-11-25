
function handleError(response, textStatus) {
	$("#loadingimage").hide();
	$("#responsBody").hide();
}
function handleSuccess(response, textStatus) {
	$("#loadingimage").hide();
	$("#responsBody").text(response);
	$("#responsBody").show();
};

function parseDoc(type) {
	var uri = 'api/text';
	$("#loadingimage").show();
	$("#responsBody").hide();

	uri = uri + "/" + type;
	var inputText = $('#textToParse').val();

	
	$.ajax(
	{
		beforeSend: function (req) {
			req.setRequestHeader("Accept", "text/csv; charset=utf-8");//, application/xml;q=0.9
		},

		type: "POST",
		url: uri,
		data: JSON.stringify(inputText),
		contentType: "application/json",

		statusCode: {
			204: function () {
				alert("Text area is empty!");
			},
			400: function () {
				alert("Bad Request. Операция не выполнена.");
			}
		},


		success: handleSuccess,
		error: handleError
	});
}