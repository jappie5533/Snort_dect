$(function () {
    $(document).ready(function () {
        $("#MainContent_RightContent_TextBox2").live('focusout', function () {
            $("#MainContent_RightContent_TextBox3").attr('value', $("#MainContent_RightContent_TextBox2").val());
        });
    });
});

function DrawConnectionDiagram() {
    var json;
    $.ajax({
        type: 'POST',
        url: '/Admin/PeerConnectionDiagram.aspx/GetNodes',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (response) {
            json = $.parseJSON(response.d);
        }
    });

    $.each(json, function () {
        if(this.type == "hub")
            this.data = $.parseJSON("{\"$color\": \"#83548B\", \"$type\": \"circle\", \"$dim\": 5, \"$t\": \"hub\"}");
        else
            this.data = $.parseJSON("{\"$color\": \"#C74243\", \"$type\": \"star\", \"$dim\": 6, \"$t\": \"agent\"}");
    });

    var Log = {
        elem: false,
        write: function (text) {
            if (!this.elem)
                this.elem = document.getElementById('log');
            this.elem.innerHTML = text;
            this.elem.style.left = (500 - this.elem.offsetWidth / 2) + 'px';
        }
    };

    var fd = new $jit.ForceDirected({
        injectInto: 'peerconnectiondiagram',
        Navigation: {
            enable: true,
            panning: 'avoid nodes',
            zooming: 10
        },
        Node: {
            overridable: true
        },
        Edge: {
            overridable: true,
            color: '#23A4FF',
            lineWidth: 0.4
        },
        Label: {
            type: 'Native',
            size: 10,
            style: 'bold'
        },
        Tips: {
            enable: true,
            onShow: function (tip, node) {
                var count = 0;
                node.eachAdjacency(function () { count++; });
                tip.innerHTML = "<div class=\"tip-title\">" + node.name + "</div>"
              + "<div class=\"tip-text\"><b>connections:</b> " + count + "</div>";
            }
        },
        Events: {
            enable: true,
            type: 'Native',
            //Change cursor style when hovering a node
            onMouseEnter: function () {
                fd.canvas.getElement().style.cursor = 'move';
            },
            onMouseLeave: function () {
                fd.canvas.getElement().style.cursor = '';
            },
            //Update node positions when dragged
            onDragMove: function (node, eventInfo, e) {
                var pos = eventInfo.getPos();
                node.pos.setc(pos.x, pos.y);
                fd.plot();
            },
            //Implement the same handler for touchscreens
            onTouchMove: function (node, eventInfo, e) {
                $jit.util.event.stop(e); //stop default touchmove event
                this.onDragMove(node, eventInfo, e);
            },
            //Add also a click handler to nodes
            onClick: function (node) {
                if (!node) return;
                // Build the right column relations list.
                // This is done by traversing the clicked node connections.
                var html = "";

                list = [];
                node.eachAdjacency(function (adj) {
                    list.push(adj.nodeTo.name);
                });

                if (node.data.$t == "hub") {
                    html += "<b>IP: </b><h4 style=\"display: inline\">" + node.name + "</h4><br />";
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/PeerConnectionDiagram.aspx/GetNodeInfo',
                        data: "{ip:" + JSON.stringify(node.name) + "}",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        async: false,
                        success: function (response) {
                            html += "<b>Account: </b>" + response.d.Account + "<br />";
                            html += "<b>City: </b>" + response.d.Geo.City + "<br />";
                            html += "<b>Country: </b>" + response.d.Geo.Country + "<br />";
                        }
                    });
                } else if (node.data.$t == "agent") {
                    html += "<b style=\"display: inline\">Account:</b> " + node.name + "<br />";
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/PeerConnectionDiagram.aspx/GetUNodeInfo',
                        data: "{account:" + JSON.stringify(node.name) + "}",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        async: false,
                        success: function (response) {
                            html += "<b>E-mail: </b>" + response.d.Email + "<br />";
                            html += "<b>IP: </b>" + response.d.IP + "<br />";
                        }
                    });
                }

                html += "<b>Connections:</b><ul><li>",
                //append connections information
                $jit.id('inner-details').innerHTML = html + list.join("</li><li>") + "</li></ul>";
            }
        },
        iterations: 200,
        levelDistance: 100,
        onCreateLabel: function (domElement, node) {
            domElement.innerHTML = node.name;
            var style = domElement.style;
            style.fontSize = "0.8em";
            style.color = "#ddd";
        },
        onPlaceLabel: function (domElement, node) {
            var style = domElement.style;
            var left = parseInt(style.left);
            var top = parseInt(style.top);
            var w = domElement.offsetWidth;
            style.left = (left - w / 2) + 'px';
            style.top = (top + 10) + 'px';
            style.display = '';
        }
    });
    fd.loadJSON(json);
    // compute positions incrementally and animate.
    fd.computeIncremental({
        iter: 40,
        property: 'end',
        onStep: function (perc) {
            Log.write(perc + '% loaded...');
        },
        onComplete: function () {
            //Log.write('done');
            $('#log').css('display', 'none');
            fd.animate({
                modes: ['linear'],
                transition: $jit.Trans.Elastic.easeOut,
                duration: 2500
            });
        }
    });
}