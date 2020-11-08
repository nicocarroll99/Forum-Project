function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide(100);
        $('#' + confirmDeleteSpan).show(100);
    } else {
        $('#' + deleteSpan).show(100);
        $('#' + confirmDeleteSpan).hide(100);
    }
}