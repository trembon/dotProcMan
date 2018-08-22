$(() => {
    $(".output-box").each((i, el) => {
        $(el).scrollTop($(el).prop("scrollHeight"));
    });
});