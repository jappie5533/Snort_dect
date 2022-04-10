var pre_json_str;
var pre_type;
var pre_showType;
var chart;
var table;
var slider;
var dashboard;

google.load("visualization", "1.0", {
    packages: ["corechart", "controls", "table"]
});

$(document).ready(function () {
    google.setOnLoadCallback(initialize);
    eventBinding();
});

function initialize() {
    pre_type = "PieChart";
    pre_showType = "chart";
    // Define a chart
    chart = new google.visualization.ChartWrapper({
        'chartType': pre_type,
        'containerId': 'chart_div',
        'options': {
            'height': 492,
            'legend': 'right',
            'title': 'Analysis Result'
            // 'chartArea' : {
            // 'left' : 35,
            // 'top' : 20,
            // 'right' : 0,
            // 'bottom' : 0
            // }
        },
        // Instruct the piechart to use colums 1 (Source_IP) and 10 (Total_Count)
        // from the 'data' DataTable.
        'view': {
            'columns': [1, 10]
        }
    });
    table = new google.visualization.ChartWrapper({
        'chartType': 'Table',
        'containerId': 'table_div',
        'options': {
            'width': '590px',
            'height': '488px'
        }
    });
    // Define a slider control for the 'Donuts eaten' column
    slider = new google.visualization.ControlWrapper({
        'controlType': 'NumberRangeFilter',
        'containerId': 'control2',
        'options': {
            'filterColumnLabel': 'Total_Count',
            'ui': {
                'labelStacking': 'vertical'
            }
        }
    });

    // Create a dashboard
    dashboard = new google.visualization.Dashboard(document.getElementById('dashboard'));

    // Establish bindings, declaring the both the slider and the category
    // picker will drive both charts.
    dashboard.bind(slider, [table, chart]);

    google.visualization.events.addListener(dashboard, 'ready', function () {
        showControl(pre_showType);
    });

    $('input[value="chart"]').attr('checked', true);

    drawChart('', '');
}

function eventBinding() {
    $('#select_chart').bind('change', function () {
        pre_type = this.value;
        $('#chart_div').show();
        $('#table_div').show();
        drawChart(pre_json_str, this.value);
    });

    $('input[name="showControl"]').bind('click', function () {
        showControl(this.value);
    });
}

function drawChart(json_str, type) {
    if (type == '' || type == undefined)
        type = pre_type;

    // Define Data.
    if (json_str == '' || json_str == undefined) {
        //data = new google.visualization.DataTable('{"cols": [{"id": "time", "label": "time", "type": "string"}, {"id": "sourceip", "label": "sourceip", "type": "string"}, {"id": "protocol", "label": "protocol", "type": "string"}, {"id": "tcpflag", "label": "tcpflag", "type": "string"}, {"id": "length", "label": "length", "type": "string"}, {"id": "Total_Count", "label": "Total_Count", "type": "number"}], "rows": [{"c": [{"v": "2013-04-12 11:54:21.639"}, {"v": "1.234.83.163"}, {"v": "Tcp"}, {"v": "Synchronize"}, {"v": "60"}, {"v": 1}]}, {"c": [{"v": "2013-04-12 08:04:18.736"}, {"v": "112.175.243.36"}, {"v": "Tcp"}, {"v": "Synchronize"}, {"v": "60"}, {"v": 1}]}, {"c": [{"v": "2013-04-12 04:25:45.083"}, {"v": "114.113.227.174"}, {"v": "Tcp"}, {"v": "Synchronize"}, {"v": "60"}, {"v": 1}]}]}');
        return;
    }
    else {
        pre_json_str = json_str;
        data = new google.visualization.DataTable(json_str);
    }

    chart.setChartType(type);

    // Draw the entire dashboard.
    dashboard.draw(data);
}

function showControl(showType) {
    pre_showType = showType;

    if (showType == 'table') {
        $('#table_div').hide().fadeIn('slow');
        $('#chart_div').hide();
    }
    else {
        $('#chart_div').hide().fadeIn('slow');
        $('#table_div').hide();
    }
} 
