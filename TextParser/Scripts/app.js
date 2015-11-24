
function parseDoc(type) {
	var uri = 'api/text';
	$("#loadingimage").show();
	$("#responsBody").hide();

	uri = uri + "/" + type;
	var inputText = $('#textToParse').val();
	$.ajax(
	{
		type: "POST",
		url: uri,
		data: JSON.stringify(inputText),
		dataType: "json",
		contentType: "application/json",
		success: function (response) {
			$("#loadingimage").hide();
			$("#responsBody").text(response);
			$("#responsBody").show();
		},
		error: function (response) {
			$("#loadingimage").hide();
			alert("error", + response.Message.toString());
		}
	});
}