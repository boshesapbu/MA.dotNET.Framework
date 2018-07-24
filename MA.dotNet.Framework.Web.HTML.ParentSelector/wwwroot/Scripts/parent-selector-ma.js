/* OVERRIDES BEGIN */
Array.prototype.indexOf || (Array.prototype.indexOf = function (d, e) {
    var a;
    if (null == this) throw new TypeError('"this" is null or not defined');
    var c = Object(this),
        b = c.length >>> 0;
    if (0 === b) return -1;
    a = +e || 0;
    Infinity === Math.abs(a) && (a = 0);
    if (a >= b) return -1;
    for (a = Math.max(0 <= a ? a : b - Math.abs(a), 0); a < b;) {
        if (a in c && c[a] === d) return a;
        a++
    }
    return -1
});
/* OVERRIDES END */

(function () {

    var observeCreateEvent = (function () {
        var MutationObserver = window.MutationObserver || window.WebKitMutationObserver;

        return function (obj, callback) {
            if (!obj || !obj.nodeType === 1) return; // validation

            // define a new observer
            var obs = new MutationObserver(function (mutations, observer) {
                callback(mutations[0]);
            });
            // have the observer observe foo for changes in children
            obs.observe(obj,
                {
                    childList: true,
                    subtree: true,
                    attributes: true,
                    characterData: true
                });
        }
    })();

    var waitProcess = false;
    var selectorsWithoutEvent = [];
    var selectorsWithEvent = [];

    function observerCallBack(m) {
        if (waitProcess == true) {
            return;
        }
        waitProcess = true;

        var funcCheckNewSelectors = function () {
            if (m != null && m.addedNodes.length > 0) {
                for (var addedNodeIndex = 0; addedNodeIndex < m.addedNodes.length; addedNodeIndex++) {
                    var addedNode = m.addedNodes[addedNodeIndex];

                    // Find selector elem
                    if (addedNode.tagName == "MA-PARENT-SELECTOR") {

                        // If without event
                        if (addedNode.getAttribute("event") == null) {
                            selectorsWithoutEvent.push(addedNode);
                        }
                        // If with event
                        else {
                            selectorsWithEvent.push(addedNode);
                        }

                    }
                }
            }

            if (m != null && m.removedNodes.length > 0) {
                for (var removedNodeIndex = 0; removedNodeIndex < m.removedNodes.length; removedNodeIndex++) {
                    var removedNode = m.removedNodes[removedNodeIndex];

                    // Find selector elem
                    if (removedNode.tagName == "MA-PARENT-SELECTOR") {

                        // If without event
                        if (removedNode.getAttribute("event") == null) {
                            var removedIndex = selectorsWithoutEvent.indexOf(removedNode);
                            if (removedIndex > -1) {
                                // Removed before added classes
                                var selector = selectorsWithoutEvent[removedIndex];

                                var elem = selector.getAttribute("elem");
                                var addClass = selector.getAttribute("add-class");

                                var elemsDOM = document.querySelectorAll(elem);
                                for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                    elemsDOM[elemIndex].classList.remove(addClass);
                                }

                                selectorsWithoutEvent.splice(removedIndex, 1);
                            }
                        }
                        // If with event
                        else {
                            var removedIndex = selectorsWithEvent.indexOf(removedNode);
                            if (removedIndex > -1) {
                                // Removed before added classes
                                var eventSelector = selectorsWithoutEvent[removedIndex];

                                var elem = eventSelector.getAttribute("elem");
                                var addClass = eventSelector.getAttribute("add-class");

                                var elemsDOM = document.querySelectorAll(elem);
                                for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                    elemsDOM[elemIndex].classList.remove(addClass);
                                }

                                selectorsWithEvent.splice(removedIndex, 1);
                            }
                        }

                    }
                }
            }
        }
        funcCheckNewSelectors();

        var selectors = selectorsWithoutEvent;
        for (var selectorIndex = 0; selectorIndex < selectors.length; selectorIndex++) {
            var selector = selectors[selectorIndex];

            // Getting attributes
            var search = selector.getAttribute("search");
            var elem = selector.getAttribute("elem");
            var addClass = selector.getAttribute("add-class");

            var elemsDOM = document.querySelectorAll(elem);
            if (document.querySelector(search) != null) {
                for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                    elemsDOM[elemIndex].classList.add(addClass);
                }
            }
            else {
                for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                    elemsDOM[elemIndex].classList.remove(addClass);
                }
            }
        }

        var eventSelectors = selectorsWithEvent;
        for (var eventSelectorIndex = 0; eventSelectorIndex < eventSelectors.length; eventSelectorIndex++) {
            var eventSelector = eventSelectors[eventSelectorIndex];

            // Getting attributes
            var eventName = eventSelector.getAttribute("event");
            var search = eventSelector.getAttribute("search");
            var elem = eventSelector.getAttribute("elem");
            var addClass = eventSelector.getAttribute("add-class");

            // Bind event
            var func = function (eventName, search, elem, addClass) {
                var searchElemsDOM = document.querySelectorAll(search);

                var funcSearchElem = function (searchElemDOM) {
                    // Bind default events - BEGIN
                    if (searchElemDOM != null) {
                        if (searchElemDOM.PARENT_SELECTOR_EVENTS_LOADED != true) {
                            searchElemDOM.PARENT_SELECTOR_EVENTS_LOADED = true;
                            searchElemDOM.PARENT_SELECTOR_EVENTS_SELECTOR_ITEMS = [];

                            var funcLoadHover = function (searchElemDOM) {
                                searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER_EVENTS = [];
                                searchElemDOM.addEventListener("mousemove", function () {
                                    if (searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER == true) {
                                        return;
                                    }
                                    searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER = true;

                                    for (var i = 0; i < searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER_EVENTS.length; i++) {
                                        searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER_EVENTS[i].call(searchElemDOM, true);
                                    }
                                });
                                searchElemDOM.addEventListener("mouseleave", function () {
                                    searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER = false;

                                    for (var i = 0; i < searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER_EVENTS.length; i++) {
                                        searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER_EVENTS[i].call(searchElemDOM, false);
                                    }
                                });
                            }
                            funcLoadHover(searchElemDOM);

                            var funcLoadClick = function (searchElemDOM) {
                                searchElemDOM.PARENT_SELECTOR_MOUSE_CLICK_EVENTS = [];
                                searchElemDOM.addEventListener("mousedown", function () {
                                    for (var i = 0; i < searchElemDOM.PARENT_SELECTOR_MOUSE_CLICK_EVENTS.length; i++) {
                                        searchElemDOM.PARENT_SELECTOR_MOUSE_CLICK_EVENTS[i].call(searchElemDOM, true);
                                    }
                                });

                                searchElemDOM.addEventListener("mouseup", function () {
                                    for (var i = 0; i < searchElemDOM.PARENT_SELECTOR_MOUSE_CLICK_EVENTS.length; i++) {
                                        searchElemDOM.PARENT_SELECTOR_MOUSE_CLICK_EVENTS[i].call(searchElemDOM, false);
                                    }
                                });
                            }
                            funcLoadClick(searchElemDOM);

                            var funcLoadKeyPress = function (searchElemDOM) {
                                searchElemDOM.PARENT_SELECTOR_KEY_PRESS_EVENTS = [];
                                searchElemDOM.addEventListener("keydown", function () {
                                    for (var i = 0; i < searchElemDOM.PARENT_SELECTOR_KEY_PRESS_EVENTS.length; i++) {
                                        searchElemDOM.PARENT_SELECTOR_KEY_PRESS_EVENTS[i].call(searchElemDOM, true);
                                    }
                                });

                                searchElemDOM.addEventListener("keyup", function () {
                                    for (var i = 0; i < searchElemDOM.PARENT_SELECTOR_KEY_PRESS_EVENTS.length; i++) {
                                        searchElemDOM.PARENT_SELECTOR_KEY_PRESS_EVENTS[i].call(searchElemDOM, false);
                                    }
                                });
                            }
                            funcLoadKeyPress(searchElemDOM);

                            var funcLoadFocus = function (searchElemDOM) {
                                searchElemDOM.PARENT_SELECTOR_FOCUS_EVENTS = [];
                                searchElemDOM.addEventListener("focusin", function () {
                                    for (var i = 0; i < searchElemDOM.PARENT_SELECTOR_FOCUS_EVENTS.length; i++) {
                                        searchElemDOM.PARENT_SELECTOR_FOCUS_EVENTS[i].call(searchElemDOM, true);
                                    }
                                });

                                searchElemDOM.addEventListener("focusout", function () {
                                    for (var i = 0; i < searchElemDOM.PARENT_SELECTOR_FOCUS_EVENTS.length; i++) {
                                        searchElemDOM.PARENT_SELECTOR_FOCUS_EVENTS[i].call(searchElemDOM, false);
                                    }
                                });
                            }
                            funcLoadFocus(searchElemDOM);
                        }
                        // Bind default events - END

                        if (searchElemDOM.PARENT_SELECTOR_EVENTS_SELECTOR_ITEMS.indexOf(eventSelector) == -1) {
                            searchElemDOM.PARENT_SELECTOR_EVENTS_SELECTOR_ITEMS.push(eventSelector);

                            switch (eventName) {
                                case "hover": {
                                    searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER_EVENTS.push(function (hover) {
                                        if (hover == true) {
                                            var elemsDOM = document.querySelectorAll(elem);
                                            for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                                elemsDOM[elemIndex].classList.add(addClass);
                                            }
                                        }
                                        else {
                                            var elemsDOM = document.querySelectorAll(elem);
                                            for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                                elemsDOM[elemIndex].classList.remove(addClass);
                                            }
                                        }
                                    });
                                }
                                    break;
                                case "click": {
                                    searchElemDOM.PARENT_SELECTOR_MOUSE_CLICK_EVENTS.push(function (down) {
                                        if (down == true) {
                                            var elemsDOM = document.querySelectorAll(elem);
                                            for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                                elemsDOM[elemIndex].classList.add(addClass);
                                            }
                                        }
                                        else {
                                            var elemsDOM = document.querySelectorAll(elem);
                                            for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                                elemsDOM[elemIndex].classList.remove(addClass);
                                            }
                                        }
                                    });
                                }
                                    break;
                                case "keypress": {
                                    searchElemDOM.PARENT_SELECTOR_KEY_PRESS_EVENTS.push(function (down) {
                                        if (down == true) {
                                            var elemsDOM = document.querySelectorAll(elem);
                                            for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                                elemsDOM[elemIndex].classList.add(addClass);
                                            }
                                        }
                                        else {
                                            var elemsDOM = document.querySelectorAll(elem);
                                            for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                                elemsDOM[elemIndex].classList.remove(addClass);
                                            }
                                        }
                                    });
                                }
                                    break;
                                case "focus": {
                                    searchElemDOM.PARENT_SELECTOR_FOCUS_EVENTS.push(function (focusin) {
                                        if (focusin == true) {
                                            var elemsDOM = document.querySelectorAll(elem);
                                            for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                                elemsDOM[elemIndex].classList.add(addClass);
                                            }
                                        }
                                        else {
                                            var elemsDOM = document.querySelectorAll(elem);
                                            for (var elemIndex = 0; elemIndex < elemsDOM.length; elemIndex++) {
                                                elemsDOM[elemIndex].classList.remove(addClass);
                                            }
                                        }
                                    });
                                }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                for (var searchElemIndex = 0; searchElemIndex < searchElemsDOM.length; searchElemIndex++) {
                    funcSearchElem(searchElemsDOM[searchElemIndex]);
                }
            }
            func(eventName, search, elem, addClass);
        }

        setTimeout(function () {
            waitProcess = false;
        });
    }
    observeCreateEvent(document.body, observerCallBack);

    function init() {
        var selectors = document.querySelectorAll("ma-parent-selector:not([event])");
        for (var selectorIndex = 0; selectorIndex < selectors.length; selectorIndex++) {
            selectorsWithoutEvent.push(selectors[selectorIndex]);
        }
        var eventSelectors = document.querySelectorAll("ma-parent-selector[event]");
        for (var eventSelectorIndex = 0; eventSelectorIndex < eventSelectors.length; eventSelectorIndex++) {
            selectorsWithEvent.push(eventSelectors[eventSelectorIndex]);
        }
    }
    init();
})();