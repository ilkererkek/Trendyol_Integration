

var stackSize = 1;
$(document).on('change', '.category-select', (e) => {
    const id = $(e.target).val();
    const index = $(e.target).attr('id');
    deleteSelects(parseInt(index))
    $.ajax({
        type: 'GET',
        url: `/Categories/GetSubCategories/${id}`,
        dataType: 'json',
        success: function (data) {
            if (data.data.length != 0) {

                createSelect(index, data.data)
            }
            else {
                $('.category-input').val(id);
                getAttributeList(id)
            }
        },
        error: function (data) {

        }
    })

})

const deleteSelects = (index) => {
    for (var i = index + 1; i < stackSize; i++) {
        $('#' + i).remove()
    }
}
const createSelect = (stackIndex,categories) => {
    stackSize++;
    stackIndex++;
    var html = `<div class="form-group"><select class="category-select form-control" name="categories" id="${stackIndex}"><option disabled selected value> Alt Kategori Seçiniz </option>`
    for (var i = 0; i < categories.length; i++) {
        html =  html.concat(`<option value="${categories[i].CategoryId}">${categories[i].CategoryName} </option>`)
    }
    html = html.concat(`</select></div>`)
    $('.select').append(html)
}

const getAttributeList = (id) => {
    $.ajax({
        type: 'GET',
        url: `/Categories/GetAttributes/${id}`,
        dataType: 'json',
        success: function (data) {
            console.log(JSON.parse(data))

        },
        error: function (data) {

        }
    })
}

const createAttributeSelection = () => {

}