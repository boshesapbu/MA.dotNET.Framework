
var observeDOMForParentSelector = (function () {
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

var waitProcessForParentSelector = false;
observeDOMForParentSelector(document.body, function (m) {
    if (waitProcessForParentSelector == true) {
        return;
    }
    waitProcessForParentSelector = true;

    var selectors = document.querySelectorAll("ma-parent-selector:not([event])");
    for (var selectorIndex = 0; selectorIndex < selectors.length; selectorIndex++) {
        var selector = selectors[selectorIndex];

        // Getting attributes
        var search = selector.getAttribute("search");
        var elem = selector.getAttribute("elem");
        var addClass = selector.getAttribute("add-class");

        if (document.querySelector(search) != null) {
            document.querySelector(elem).classList.add(addClass);
        }
        else {
            document.querySelector(elem).classList.remove(addClass);
        }
    }

    var eventSelectors = document.querySelectorAll("ma-parent-selector[event]");
    for (var eventSelectorIndex = 0; eventSelectorIndex < eventSelectors.length; eventSelectorIndex++) {
        var eventSelector = eventSelectors[eventSelectorIndex];

        // Getting attributes
        var eventName = eventSelector.getAttribute("event");
        var search = eventSelector.getAttribute("search");
        var elem = eventSelector.getAttribute("elem");
        var addClass = eventSelector.getAttribute("add-class");

        // Bind event
        var func = function (eventName, search, elem, addClass) {
            var searchElemDOM = document.querySelector(search);
            // Bind default events - BEGIN
            if (searchElemDOM != null) {
                if (searchElemDOM.PARENT_SELECTOR_EVENTS_LOADED != true) {
                    searchElemDOM.PARENT_SELECTOR_EVENTS_LOADED = true;

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
                }
                // Bind default events - END

                switch (eventName) {
                    case "hover": {
                        if (eventSelector.PARENT_SELECTOR_EVENTS_LOADED != true) {
                            eventSelector.PARENT_SELECTOR_EVENTS_LOADED = true;

                            searchElemDOM.PARENT_SELECTOR_MOUSE_HOVER_EVENTS.push(function (hover) {
                                if (hover == true) {
                                    document.querySelector(elem).classList.add(addClass);
                                }
                                else {
                                    document.querySelector(elem).classList.remove(addClass);
                                }
                            });
                        }
                    }
                        break;
                    case "click": {
                        if (eventSelector.PARENT_SELECTOR_EVENTS_LOADED != true) {
                            eventSelector.PARENT_SELECTOR_EVENTS_LOADED = true;

                            searchElemDOM.PARENT_SELECTOR_MOUSE_CLICK_EVENTS.push(function (down) {
                                if (down == true) {
                                    document.querySelector(elem).classList.add(addClass);
                                }
                                else {
                                    document.querySelector(elem).classList.remove(addClass);
                                }
                            });
                        }
                    }
                        break;
                    case "keypress": {
                        if (eventSelector.PARENT_SELECTOR_EVENTS_LOADED != true) {
                            eventSelector.PARENT_SELECTOR_EVENTS_LOADED = true;

                            searchElemDOM.PARENT_SELECTOR_KEY_PRESS_EVENTS.push(function (down) {
                                if (down == true) {
                                    document.querySelector(elem).classList.add(addClass);
                                }
                                else {
                                    document.querySelector(elem).classList.remove(addClass);
                                }
                            });
                        }
                    }
                        break;
                    default:
                        break;
                }
            }
        }
        func(eventName, search, elem, addClass);
    }

    setTimeout(function () {
        waitProcessForParentSelector = false;
    });
});