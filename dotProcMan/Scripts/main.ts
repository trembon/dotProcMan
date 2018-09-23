$(() => {
    $(".output-box").each((i, el) => {
        $(el).scrollTop($(el).prop("scrollHeight"));
    });

    $("a[data-jsclick]").click(e => {
        e.preventDefault();
        let $t = $(e.currentTarget);

        switch ($t.data("jsclick")) {
            case "start":
                process_start($t.data("processId"));
                break;

            case "stop":
                process_stop($t.data("processId"));
                break;

            case "restart":
                process_restart($t.data("processId"));
                break;
        }
    });
});

function process_start(processId: string): void {
    $.post(`/api/process/start?id=${processId}`, result => {
        if (result) {
            window.location.reload();
        } else {
            alert("Failed to start process, check output for errors.");
        }
    });
}

function process_stop(processId: string): void {
    $.post(`/api/process/stop?id=${processId}`, result => {
        if (result) {
            window.location.reload();
        } else {
            alert("Failed to stop process, check output for errors.");
        }
    });
}

function process_restart(processId: string): void {
    $.post(`/api/process/restart?id=${processId}`, result => {
        if (result) {
            window.location.reload();
        } else {
            alert("Failed to restart process, check output for errors.");
        }
    });
}