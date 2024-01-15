$(document).ready(function () {
    // 돔 로드시 호출

    // 투두리스트 데이터
    getTodoList();
    // 시간 및 날짜
    getCurrentTime();
    setInterval(getCurrentTime, 1000);
    getCurrentDate();

});

// 시간 얻기
function getCurrentTime() {

    const now = new Date();
    const hours = now.getHours();
    const minutes = now.getMinutes();
    const seconds = now.getSeconds();

    $('#nowTime').text(`현재시각 : ${hours}시 ${String(minutes).padStart(2, '0')}분 ${String(seconds).padStart(2, '0')}초`);

    if (hours === "23" && minutes === "59" && secounds === "59") {
        getCurrentDate();
    }
}

// 날짜 얻기
function getCurrentDate() {
    const now = new Date();
    const year = now.getFullYear();
    const month = now.getMonth() + 1;
    const date = now.getDate();

    // 숫자에 따른 한국어 반환
    const korean = ['일요일', '월요일', '화요일', '수요일', '목요일', '금요일', '토요일'];
    const day = korean[now.getDay()];

    $('#nowDate').text(`${year}년 ${month}월 ${date}일 ${day}`);
}

// 투두 리스트 동적처리
function drawTodoList(todoList) {
    const todoContent = $('#todoContent');

    todoContent.empty();

    if (todoList.length === 0) {
        todoContent.append('<p class="lead fw-normal mb-0 ms-3 text-center" id="todo-text">아직 추가된 투두리스트가 없습니다</p>')
    }

    todoList.forEach(function (todo) {
        let todoHtml = `
              <ul class="list-group list-group-horizontal rounded-0 bg-transparent" id="todoList">
                <!-- 내용 -->
                <li class="list-group-item px-3 py-1 d-flex align-items-center flex-grow-1 border-0 bg-transparent" id="todo-content">
                  <input class="form-check-input me-0" type="checkbox" value="${todo.todoNo}" id="todo-checkbox"  ${(todo.todoStatus == 1 ? "checked" : "")} />
                  <p class="lead fw-normal mb-0 ms-3" id="todo-text" ${(todo.todoStatus == 1 ? "style='text-decoration: line-through'" : "")}>${todo.todoContent}</p>
                </li>
                <!-- 수정 삭제 버튼 -->
                <li class="list-group-item ps-3 pe-0 py-1 rounded-0 border-0 bg-transparent">
                  <div class="text-end text-muted">
                    <button type="button" class="btn btn-primary btn-update" onclick="updateTodo(event, '${todo.todoNo}')">수정</button>
                    <button type="button" class="btn btn-primary" onclick="deleteTodo('${todo.todoNo}')">삭제</button>
                  </div>
                </li>
              </ul>
             `;

        todoContent.append(todoHtml);
        $('#todoContent').addClass('overflow-auto p-3');
        $('#todoContent').css('max-height', 340);
    });
}

// 투두 리스트 호출
function getTodoList() {
    $.ajax({
        url: "/todo/gettodolist",
        type: "Get",
        success: function (result) {
            // 값에 따라 투두 리스트를 그림
            drawTodoList(result);
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

// 투두 리스트 저장
function saveTodo() {

    let todoVal = $('#todo-input').val();

    if (todoVal === "") {
        alert("값을 입력해주세요!")
        return;
    }
    $.ajax({
        url: "/todo/put/",
        type: "Post",
        data: { "TodoContent": todoVal },
        success: function (result) {
            getTodoList();
            $('#todo-input').val("");
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
// 투두 리스트 삭제
function deleteTodo(todoNo) {
    $.ajax({
        url: "/todo/delete/" + todoNo,
        type: "Delete",
        success: function (result) {
            getTodoList();
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

// 수정창 동적처리
function updateTodo(event, todoNo) {

    const $btn = $(event.target);
    const $ul = $btn.closest('ul');
    const $li = $ul.find('li#todo-content');
    const todoVal = $li.text().trim();

    let $updateBtn = $('<button class="btn btn-success btn-save">저장</button>');
    let inputText = '<input type="text" class="form-control" value="' + todoVal + '">';

    $li.html(inputText);
    $btn.replaceWith($updateBtn);

    $updateBtn.on('click', function () {
        updateSave($li, todoNo);
    });
}

// 체크 박스 이벤트 감지
$(document).on('change', '#todo-checkbox', function () {

    const $todoText = $(this).closest('li').find('#todo-text');
    let todoNo = $(this).val();

    if ($(this).prop('checked')) {
        updateTodoStatus(todoNo, 1);
    } else {
        updateTodoStatus(todoNo, 0);
    }
});

// 투두 상태 수정
function updateTodoStatus(todoNo, todoStatus) {
    $.ajax({
        url: "/todo/update/",
        type: "Put",
        data:
        {
            "todoNo": todoNo,
            "todoStatus": todoStatus
        },
        success: function (result) {
            getTodoList();
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
// 투두 리스트 수정
function updateSave($li, todoNo) {

    const todoVal = $li.find('input').val();

    $.ajax({
        url: "/todo/update/",
        type: "Put",
        data:
        {
            "todoNo": todoNo,
            "todoContent": todoVal,
            "todoStatus": -1
        },
        success: function (result) {
            getTodoList();
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

function startRecord() {
    window.SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;

    const recognition = new SpeechRecognition();
    recognition.interimResults = true;
    recognition.lang = 'ko-KR';

    recognition.onstart = () => {
        $('#recode-Result').text("음성 인식 중입니다");
    };

    recognition.onresult = (event) => {
        const result = event.results[event.results.length - 1][0].transcript;

        console.log(result);
        $('#todo-input').val(result);
    };

    recognition.onerror = (event) => {
        console.error('음성 인식 오류', event.error);

        if (event.error === 'not-allowed') {
            alert('음성 인식 기능을 사용하려면 마이크 설정을 허용해야합니다.\n마이크 설정을 확인하세요.');
        } else {
            alert('음성 인식 기능을 사용 할 수 없습니다')
        }
    };

    recognition.onend = () => {
        console.log('음성 인식 종료');
        $('#recode-Result').text("");
    };

    recognition.start();
}

