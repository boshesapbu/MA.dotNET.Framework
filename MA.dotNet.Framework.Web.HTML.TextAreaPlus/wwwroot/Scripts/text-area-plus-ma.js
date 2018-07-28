function textAreaPlus(textareaDOM, eventsForTextAreaPlus) {
    /*VARIABLES BEGIN*/
    var mainDOM = null;
    var inputContentDOM = null;
    var rowContentDOM = null;

    // Default Parameters Fill
    eventsForTextAreaPlus.keydown = eventsForTextAreaPlus.keydown == null ? function () { } : eventsForTextAreaPlus.keydown;
    eventsForTextAreaPlus.change = eventsForTextAreaPlus.change == null ? function () { } : eventsForTextAreaPlus.change;
    /*VARIABLES END*/

    /*FUNCTIONS BEGIN*/
    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    function textAreaFocusSetSelectionStart(newTextArea, selection) {
        var focusEvent = function () {
            setTimeout(function () {
                newTextArea.setSelectionRange(selection, selection);
            });

            newTextArea.removeEventListener("focus", focusEvent);
        };

        newTextArea.addEventListener("focus", focusEvent);
        newTextArea.focus();
    }

    function textAreasFocusAndSetSelectionStart(oldTextArea, newTextArea, position) {
        if (oldTextArea.selectionEnd >= newTextArea.value.length || position == "set-last-position") {
            textAreaFocusSetSelectionStart(newTextArea, newTextArea.value.length);
        }
        else {
            textAreaFocusSetSelectionStart(newTextArea, oldTextArea.selectionEnd);
        }
    }

    function createRow() {
        var rowId = "text-area-plus-row-id--" + guid();

        // <div class="text-area-plus-row">
        var rowDOM = document.createElement("div");
        rowDOM.setAttribute("class", "text-area-plus-row");
        rowDOM.setAttribute("id", rowId);

        // <div class="text-area-plus-row-input-content-before">
        var rowInputContentBeforeDOM = document.createElement("div");
        rowInputContentBeforeDOM.setAttribute("class", "text-area-plus-row-input-content-before");
        rowDOM.appendChild(rowInputContentBeforeDOM);
        // </div>

        // <div class="text-area-plus-row-input-content">
        var rowInputContentDOM = document.createElement("div");
        rowInputContentDOM.setAttribute("class", "text-area-plus-row-input-content");
        rowDOM.appendChild(rowInputContentDOM);

        // <textarea class="text-area-plus-row-input">
        var rowInputDOM = document.createElement("textarea");
        rowInputDOM.setAttribute("class", "text-area-plus-row-input");

        rowInputDOM.addEventListener("keydown", function (event) {
            // Is enter
            if (event.which == 13) {
                event.preventDefault();

                var newRow = createRow();
                var focusTextArea = newRow.querySelector("textarea");
                rowDOM.insertAdjacentElement("afterend", newRow);

                var subStringFromThisValue = this.value.substring(this.selectionEnd);
                this.value = this.value.substring(0, this.selectionEnd);
                focusTextArea.value = subStringFromThisValue;
                textAreaFocusSetSelectionStart(focusTextArea, 0);

                eventsForTextAreaPlus.change.call(rowDOM, event, rowId);
                eventsForTextAreaPlus.change.call(newRow, event, newRow.getAttribute("id"));
            }
            // Is backspace
            else if (event.which == 8) {
                // If row is empty
                if (this.value == "") {
                    event.preventDefault();

                    // Exists previous element
                    if (rowDOM.previousElementSibling != null) {
                        var focusTextArea = rowDOM.previousElementSibling.querySelector("textarea");
                        textAreasFocusAndSetSelectionStart(this, focusTextArea, "set-last-position");

                        rowDOM.remove();
                    }
                }
                // If cursor position is begin
                else if (this.selectionEnd == 0) {

                    // Exists previous element
                    if (rowDOM.previousElementSibling != null) {
                        event.preventDefault();

                        var focusTextArea = rowDOM.previousElementSibling.querySelector("textarea");
                        focusTextArea.value += this.value;
                        textAreasFocusAndSetSelectionStart(this, focusTextArea, "set-last-position");

                        eventsForTextAreaPlus.change.call(rowDOM.previousElementSibling, event, rowDOM.previousElementSibling.getAttribute("id"));
                        rowDOM.remove();
                    }
                }
            }
            // Is delete
            else if (event.which == 46) {
                // If row is empty
                if (this.value == "") {
                    event.preventDefault();

                    // Exists next element
                    if (rowDOM.nextElementSibling != null) {
                        var focusTextArea = rowDOM.nextElementSibling.querySelector("textarea");
                        textAreasFocusAndSetSelectionStart(this, focusTextArea, null);

                        rowDOM.remove();
                    }
                }
                // If cursor position is begin
                else if (this.selectionEnd == this.value.length) {

                    // Exists next element
                    if (rowDOM.nextElementSibling != null) {
                        event.preventDefault();
                        var nextTextArea = rowDOM.nextElementSibling.querySelector("textarea");
                        var beforeSelectionEnd = this.selectionEnd;
                        this.value += nextTextArea.value;
                        this.setSelectionRange(beforeSelectionEnd, beforeSelectionEnd);

                        rowDOM.nextElementSibling.remove();
                        eventsForTextAreaPlus.change.call(rowDOM, event, rowId);
                    }
                }
            }
            /*ARROWS BEGIN*/
            // Is up arrow
            else if (event.which == 38) {
                // Exists previous element
                if (rowDOM.previousElementSibling != null) {
                    var focusTextArea = rowDOM.previousElementSibling.querySelector("textarea");
                    textAreasFocusAndSetSelectionStart(this, focusTextArea, null);
                }
            }
            // Is down arrow
            else if (event.which == 40) {
                // Exists next element
                if (rowDOM.nextElementSibling != null) {
                    var focusTextArea = rowDOM.nextElementSibling.querySelector("textarea");
                    textAreasFocusAndSetSelectionStart(this, focusTextArea, null);
                }
            }
            /*ARROWS END*/


            /*EVENTS BEGIN*/
            eventsForTextAreaPlus.keydown.call(rowDOM, event, rowId);
            /*EVENTS BEGIN*/
        });
        var beforeRowInputDOMText = null;
        rowInputDOM.addEventListener("input", function (event) {
            this.value = this.value.split("\r").join("");
            var rows = this.value.split("\n");
            if (rows.length > 1) {
                this.value = rows[0];
                for (var i = rows.length - 1; i >= 1; i--) {
                    var newRow = createRow();
                    var focusTextArea = newRow.querySelector("textarea");
                    rowDOM.insertAdjacentElement("afterend", newRow);

                    focusTextArea.value = rows[i];
                    if (i == rows.length - 1)
                        textAreaFocusSetSelectionStart(focusTextArea, focusTextArea.value.length);
                    eventsForTextAreaPlus.change.call(newRow, event, newRow.getAttribute("id"));
                }
            }

            /*EVENTS BEGIN*/
            if (beforeRowInputDOMText != this.value) {
                eventsForTextAreaPlus.change.call(rowDOM, event, rowId);
                beforeRowInputDOMText = this.value;
            }
            /*EVENTS END*/
        });
        rowInputContentDOM.appendChild(rowInputDOM);
        // </textarea>

        // </div>

        // <div class="text-area-plus-row-input-content-after">
        var rowInputContentAfterDOM = document.createElement("div");
        rowInputContentAfterDOM.setAttribute("class", "text-area-plus-row-input-content-after");
        rowDOM.appendChild(rowInputContentAfterDOM);
        // </div>

        // </div>

        return rowDOM;
    }
    /*FUNCTIONS END*/

    // <div class="text-area-plus">
    mainDOM = document.createElement("div");
    mainDOM.setAttribute("class", "text-area-plus");
    textareaDOM.insertAdjacentElement("beforebegin", mainDOM);

    // <div class="text-area-plus-input-content">
    inputContentDOM = document.createElement("div");
    inputContentDOM.setAttribute("class", "text-area-plus-input-content");
    mainDOM.appendChild(inputContentDOM);

    inputContentDOM.appendChild(textareaDOM);
    // </div>

    // <div class="text-area-plus-rows-content">
    rowContentDOM = document.createElement("div");
    rowContentDOM.setAttribute("class", "text-area-plus-rows-content");
    mainDOM.appendChild(rowContentDOM);

    rowContentDOM.appendChild(createRow());

    // </div>

    // </div>


}