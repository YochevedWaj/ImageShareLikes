$(() => {

    setInterval(setLikes, 1000);

    $('#like-button').on('click', function () {
        const id = $('#image-id').val();
        $.post('/home/likeimage', { id }, function () {
            $('#like-button').prop('disabled', true);
            setLikes();
        });

    })

    function setLikes() {
        const id = $('#image-id').val();
        $.get('/home/getiamgelikes', { id }, function (likes) {
            $('#likes-count').html(likes);
        })
    };
});