$(() => {
    $(".output-box").each((i, el) => {
        $(el).scrollTop($(el).prop("scrollHeight"));
    });

    $("a[data-jsclick]").click(e => {
        e.preventDefault();
        let $t = $(e.currentTarget);

        switch ($t.data("jsclick")) {
            case "start":
                start($t.data("processId"));
                break;

            case "stop":
                stop($t.data("processId"));
                break;

            case "restart":
                restart($t.data("processId"));
                break;
        }
    });
});

function start(processId: string): void {
    $.post(`/api/process/start?id=${processId}`, result => {
        if (result) {
            window.location.reload();
        } else {
            alert("Failed to start process, check output for errors.");
        }
    });
}

function stop(processId: string): void {
    $.post(`/api/process/stop?id=${processId}`, result => {
        if (result) {
            window.location.reload();
        } else {
            alert("Failed to stop process, check output for errors.");
        }
    });
}

function restart(processId: string): void {
    $.post(`/api/process/restart?id=${processId}`, result => {
        if (result) {
            window.location.reload();
        } else {
            alert("Failed to restart process, check output for errors.");
        }
    });
}