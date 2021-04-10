
 
var stackSize = 1;
$(document).on('change', '.category-select', (e) => {
    const id = $(e.target).val();
    var index = parseInt($(e.target).attr('id'));
    deleteSelects(index)
    index = index+1
    $.ajax({
        type: 'GET',
        url: `/Categories/GetSubCategories/${id}`,
        dataType: 'html',
        data: {
            index
        },
        success: function (data) {

            if (data.toString() !== '') {
                stackSize++
                $('.select').append(data)
            }
            else {
                $('.category-input').val(id);
            }
        },
        error: function (data) {

        }
    })

})
$(document).on('keyup', '.brandname-input', (e) => {
    
    var name = $(e.target).val();
    $.ajax({
        type: 'GET',
        url: `/Products/Brands?name=${name}`,
        dataType: 'html',
        success: function (data) {
            $('.search-results').html(data)
        },
        error: function (data) {

        }
    })
})

$(document).on('click', '.brand-button', (e) => {
    const id = $(e.target).attr('id');
    const name = $(e.target).text();
    $('.brandname-input').val(name)
    $('.brandid-input').val(parseInt(id))
    $('.search-results').html('')
})

const deleteSelects = (index) => {
    for (var i = index + 1; i < stackSize; i++) {
        $('#' + i).remove()
    }
    $('.category-input').val(0);
}

