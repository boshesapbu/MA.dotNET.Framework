function tvYoutube(options) {
    // VARIABLES BEGIN
    // Bütün katmanlar detayları ile burda
    var varTvYoutubeLayers = null;
    // Eğer disable ise mouse çalışmaz
    var varTvYoutubeScrollDisable = false;
    var varTvYoutubeScrollDisable2 = false;
    // Animasyonun başlama scroll değeri
    var varTvYoutubeStartScrollTopValue = 250;
    var varTvYoutubeMovableLayer = null;
    var varTvYoutubeMovableLayerContent = null;
    var varTvYoutubeMovableLayerContentSub = null;
    var varTvYoutubeActiveIndex = 0;
    var varTvYoutubeEnd = false;
    // VARIABLES END

    // Init events
    function tvYoutubeLoad(options) {
        var layers = null;
        var movableLayer = null;
        var funcLoadLayers = function () {
            movableLayer = document.querySelectorAll("tv-youtube-movable-layer")[0];
            movableLayer.TV_YOUTUBE_COMP_TOP = 0;
            varTvYoutubeMovableLayer = movableLayer;
            varTvYoutubeMovableLayerContent = movableLayer.querySelectorAll("tv-youtube-movable-layer-content")[0];
            varTvYoutubeMovableLayerContentSub = varTvYoutubeMovableLayerContent.querySelectorAll("tv-youtube-movable-layer-content-sub")[0];
            layers = document.querySelectorAll("tv-youtube-layer");
            varTvYoutubeLayers = [];

            for (var layerIndex = 0; layerIndex < layers.length; layerIndex++) {
                var layerItem = layers[layerIndex];
                var content = layerItem.querySelectorAll("tv-youtube-layer-content")[0];
                layerItem.setAttribute("data-index", layerIndex);

                // ATTRS READ BEGIN
                var layerDataId = layerItem.getAttribute("data-id");
                var layerDataWidth = layerItem.getAttribute("data-width");
                var layerDataHeight = layerItem.getAttribute("data-height");
                var layerDataBorderRadius = layerItem.getAttribute("data-border-radius");
                var layerDataPadding = layerItem.getAttribute("data-padding");
                var layerDataX = layerItem.getAttribute("data-x");
                var layerDataY = layerItem.getAttribute("data-y");
                // ATTRS READ END

                // LAYER CSS EDIT BEGIN
                var layerStyle = layerItem.getAttribute("style");
                layerStyle = layerStyle == null ? "" : layerStyle;

                layerStyle += ";";
                layerStyle += "width:" + layerDataWidth + ";";
                layerStyle += "height:" + layerDataHeight + ";";
                content.setAttribute("style", layerStyle);

                layerStyle += "border-radius:" + layerDataBorderRadius + ";";
                layerStyle += "padding:" + layerDataPadding + ";";
                layerStyle += "left:" + layerDataX + ";";
                layerStyle += "top:" + layerDataY + ";";

                layerItem.setAttribute("style", layerStyle);
                // LAYER CSS EDIT END

                // ATTRS REMOVE AND CACHE BEGIN
                layerItem.setAttribute("data-opacity", "");
                layerItem.TV_YOUTUBE_COMP_DATA =
                    {
                        dataId: layerDataId,
                        dataWidth: layerDataWidth,
                        dataHeight: layerDataHeight,
                        dataBorderRadius: layerDataBorderRadius,
                        dataPadding: layerDataPadding,
                        dataX: layerDataX,
                        dataY: layerDataY,
                        dataIndex: layerIndex,
                        dataOffsetTop: layerItem.offsetTop
                    };
                layerItem.removeAttribute("data-opacity");
                layerItem.removeAttribute("data-id");
                layerItem.removeAttribute("data-width");
                layerItem.removeAttribute("data-height");
                layerItem.removeAttribute("data-border-radius");
                layerItem.removeAttribute("data-padding");
                layerItem.removeAttribute("data-x");
                layerItem.removeAttribute("data-y");
                // ATTRS REMOVE AND CACHE END

                varTvYoutubeLayers.push(
                    {
                        layerDOM: layerItem,
                        contentDOM: content,
                        data: layerItem.TV_YOUTUBE_COMP_DATA
                    }
                );
            }
            
            if (layers.length > 0) {
                tvYoutubeSetAnimationLayerFromId(layers[0].TV_YOUTUBE_COMP_DATA.dataId);
            } 
        }; funcLoadLayers();

        var funcLoadPageOptions = function () {
            if (options.defaultScrollTopZero == null || options.defaultScrollTopZero == true) {
                document.body.setAttribute("tv-youtube-disable", "");
                setTimeout(function () {
                    document.body.removeAttribute("tv-youtube-disable");
                }, 100);
            }
            if (options.animationActiveScroll != null) {
                varTvYoutubeStartScrollTopValue = options.animationActiveScroll;
            }

            window.addEventListener("scroll", function (e) {
                if (varTvYoutubeEnd == true) {
                    return;
                }
                // If scroll is disable then mouse should not works
                if (varTvYoutubeScrollDisable == false && varTvYoutubeScrollDisable2 == false) {

                    var startScrollControlValue = (window.innerHeight * (varTvYoutubeActiveIndex + 1)) - varTvYoutubeStartScrollTopValue;
                    if (window.scrollY >= startScrollControlValue && varTvYoutubeScrollDisable == false && varTvYoutubeScrollDisable2 == false) {
                        tvYoutubeSetAnimationLayerFromId(varTvYoutubeLayers[varTvYoutubeActiveIndex + 1].data.dataId);
                    }
                    startScrollControlValue = (window.innerHeight * varTvYoutubeActiveIndex);
                    var movableLayerTop = varTvYoutubeMovableLayer.offsetTop;
                    if (window.scrollY < startScrollControlValue && varTvYoutubeScrollDisable == false && varTvYoutubeScrollDisable2 == false) {
                        varTvYoutubeMovableLayer.style.top = varTvYoutubeMovableLayer.TV_YOUTUBE_COMP_TOP + (startScrollControlValue - window.scrollY) + "px";
                    }
                    else if (varTvYoutubeScrollDisable == false && varTvYoutubeScrollDisable2 == false) {
                        varTvYoutubeMovableLayer.style.top = varTvYoutubeMovableLayer.TV_YOUTUBE_COMP_TOP + "px";
                    }
                }
            });

            document.addEventListener("mousewheel", function (e) {
                if (varTvYoutubeEnd == true) {
                    return;
                }
                // If scroll is disable then mouse should not works
                if (varTvYoutubeScrollDisable == true) {
                    e.preventDefault();
                }
            });
        }; funcLoadPageOptions();
    }

    // Searching on varTvYoutubeLayers via Layer Data Id and return result
    function tvYoutubeGetLayerFromId(layerId) {
        for (var layerIndex = 0; layerIndex <= varTvYoutubeLayers.length; layerIndex++) {
            var layerItem = varTvYoutubeLayers[layerIndex];
            if (layerItem.data.dataId == layerId)
                return layerItem;
        }

        return null;
    }

    // Searching on varTvYoutubeLayers via Layer Data Index and return result
    function tvYoutubeGetLayerFromIndex(layerIndexP) {
        for (var layerIndex = 0; layerIndex <= varTvYoutubeLayers.length; layerIndex++) {
            var layerItem = varTvYoutubeLayers[layerIndex];
            if (layerItem.data.dataIndex == layerIndexP)
                return layerItem;
        }
        // Hiç bir değer bulunamassa
        return null;
    }
    
    // Set animation state to active via Layer Data Id
    function tvYoutubeSetAnimationLayerFromId(layerId) {
        var layer = tvYoutubeGetLayerFromId(layerId);

        var pageScrollTopBegin = window.innerHeight * layer.data.dataIndex;



        var movableLayer_NewScrollValue = (layer.data.dataOffsetTop - pageScrollTopBegin);

        varTvYoutubeScrollDisable2 = true;
        tvYoutubeTimerValue(varTvYoutubeMovableLayer.TV_YOUTUBE_COMP_TOP, movableLayer_NewScrollValue, 0.4, 1,
            // funcSet
            function (beforeValue, newValue) {
                varTvYoutubeMovableLayer.style.top = newValue + "px";
                var newAddValue = ((movableLayer_NewScrollValue - newValue) / 80);
                if (newAddValue < 0) {
                    newAddValue *= -1;
                }
                return newAddValue + 0.2;
            },
            // funcEnd
            function () {
                varTvYoutubeScrollDisable2 = false;
                varTvYoutubeMovableLayer.TV_YOUTUBE_COMP_TOP = movableLayer_NewScrollValue;
            });
        varTvYoutubeActiveIndex = layer.data.dataIndex;

        // If it is first layer then animation should be active
        var firstLayer = layer.data.dataIndex == 0;
        if (firstLayer == true) {
            varTvYoutubeMovableLayerContentSub.appendChild(layer.contentDOM);
            varTvYoutubeMovableLayer.setAttribute("style", layer.layerDOM.getAttribute("style"));
        }
        else {
            var scrollTop = pageScrollTopBegin - varTvYoutubeStartScrollTopValue;

            varTvYoutubeScrollDisable = true;

            var layerBefore = tvYoutubeGetLayerFromIndex(layer.data.dataIndex - 1);
            varTvYoutubeMovableLayerContentSub.appendChild(layer.contentDOM);

            // ANIMATION BEGIN

            var animationAngle = 90;
            varTvYoutubeMovableLayer.setAttribute("style", layer.layerDOM.getAttribute("style").replace("top:", "disableTop:") + "top:" + varTvYoutubeMovableLayer.style.top + ";");
            layer.contentDOM.style.left = layerBefore.data.dataWidth;
            layer.contentDOM.style.top = "calc(100%)";
            layer.contentDOM.style.transform = "rotateZ(" + (parseFloat(animationAngle) * -1) + "deg)";

            varTvYoutubeMovableLayer.style.transform = "rotateZ(" + animationAngle + "deg) translateX(calc( (" + layer.data.dataHeight + " / 2) - (" + layer.data.dataWidth + " / 2) )) translateY(calc( (" + layer.data.dataHeight + " / 2) - (" + layer.data.dataWidth + " / 2) ))";
            varTvYoutubeMovableLayer.style.width = layer.data.dataHeight;
            varTvYoutubeMovableLayer.style.height = layer.data.dataWidth;
            varTvYoutubeMovableLayerContentSub.style.marginLeft = "calc(-" + layerBefore.data.dataWidth + ")";
            // ANIMATION END

            window.scrollTo(0, scrollTop);
            tvYoutubeTimerValue(scrollTop, pageScrollTopBegin, 0.2, 1,
                // funcSet
                function (beforeValue, newValue) {
                    window.scrollTo(0, newValue);
                    var newAddValue = ((pageScrollTopBegin - newValue) / 80);
                    if (newAddValue < 0) {
                        newAddValue *= -1;
                    }
                    return 0.2 + newAddValue;
                },
                // funcEnd
                function () {
                    layerBefore.layerDOM.appendChild(layerBefore.contentDOM);
                    layerBefore.layerDOM.setAttribute("data-visible", "");

                    if (varTvYoutubeActiveIndex + 1 == varTvYoutubeLayers.length) {
                        layer.layerDOM.appendChild(layer.contentDOM);
                        layer.layerDOM.setAttribute("data-visible", "");
                        varTvYoutubeEnd = true;
                        varTvYoutubeMovableLayer.remove();
                    }

                    var beforeTransition = layer.contentDOM.style.transition;
                    layer.contentDOM.style.transition = "all 0s";
                    layer.contentDOM.style.marginLeft = "";
                    layer.contentDOM.style.left = "";
                    layer.contentDOM.style.top = "";
                    layer.contentDOM.style.transform = "";
                    setTimeout(
                        function () {
                            layer.contentDOM.style.transition = "";
                        }, 500
                    );

                    beforeTransition = varTvYoutubeMovableLayerContentSub.style.transition;
                    varTvYoutubeMovableLayerContentSub.style.transition = "all 0s";
                    varTvYoutubeMovableLayerContentSub.style.marginLeft = "";
                    setTimeout(
                        function () {
                            varTvYoutubeMovableLayerContentSub.style.transition = "";
                        }, 500
                    );

                    beforeTransition = varTvYoutubeMovableLayer.style.transition;
                    varTvYoutubeMovableLayer.setAttribute("style", "transition:all 0s;" + layer.layerDOM.getAttribute("style").replace("top:", "disableTop:") + "top:" + varTvYoutubeMovableLayer.style.top + ";");
                    setTimeout(
                        function () {
                            varTvYoutubeMovableLayer.style.transition = "";
                        }, 500
                    );

                    setTimeout(
                        function () {
                            varTvYoutubeScrollDisable = false;
                        }, 500
                    );
                });
        }
    }
    
    /*
        startValue : Begin Value
        endValue : End Value
        addValue : Added Value to start value
        interval : Timer interval
        funcSet : function(beforeValue, newValue)
            beforeValue : Value before added
            newValue : Value after added
        funcEnd : Run when ended
    */
    function tvYoutubeTimerValue(startValue, endValue, addValue, interval, funcSet, funcEnd) {
        var neg = endValue < startValue;
        if (neg == true && addValue > 0) {
            addValue *= -1;
        }
        var beforeValue = startValue;
        var funcTimer = function () {
            var newValue = beforeValue + addValue;
            if ((newValue <= endValue && neg == false) || (newValue >= endValue && neg == true)) {
                var newAddValue = funcSet(beforeValue, newValue);
                beforeValue = newValue;
                if (newAddValue != null) {
                    addValue = newAddValue;
                    if (neg == true && addValue > 0) {
                        addValue *= -1;
                    }
                }
                setTimeout(funcTimer, interval);
            }
            else {
                funcEnd();
            }
        };

        funcSet(startValue, startValue);
        setTimeout(funcTimer, interval);
    }
    
    tvYoutubeLoad(options);
}