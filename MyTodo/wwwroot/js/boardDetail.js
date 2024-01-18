function Delete(boardNo) {
	$.ajax({
		url: "/board/delete/" + boardNo,
		type: "Delete",
		success: function (result) {
			alert("삭제 되었습니다.");
			location.href = "/board/Index"
		},
		error: function (xhr, status) {
			if (xhr.status === 401) {
				location.href = "/User/login"
			}
			else {
				location.href = "/Error/Error500"
			}
		}
	});
}

function saveReply(boardNo) {

	let replyVal = $('#reply-input').val();

	if (replyVal === "") {
		alert("댓글 값을 입력해주세요!");
		return;
	}

	$.ajax({
		url: "/reply/put",
		type: "Post",
		data: {
			ReplyContent: replyVal,
			BoardNo: boardNo
		},
		success: function (result) {
			alert("댓글 등록 완료");
			location.reload();
		},

		error: function (xhr, status) {
			if (xhr.status === 401) {
				alert("댓글 작성을 위해서는 로그인이 필요합니다");
				location.href = "/User/login"
			}
			else {
				location.href = "/Error/Error500"
			}
		}
	});
}

function deleteReply(replyNo) {
	$.ajax({
		url: "/reply/delete/" + replyNo,
		type: "Delete",
		success: function (result) {
			alert("댓글 삭제 완료");
			location.reload();
		},
		error: function (xhr, status) {
			 if (xhr.status === 401) {
			 	location.href = "/User/login"
			 }
			 else {
			 	location.href = "/Error/Error500"
			 }
		}
	})
}