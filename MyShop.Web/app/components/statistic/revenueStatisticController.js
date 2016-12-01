(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService', '$filter'];

    function revenueStatisticController($scope, apiService, notificationService, $filter) {

        $scope.fromDate = '01/01/2016';
        $scope.toDate = '01/01/2020';


        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Doanh số', 'Lợi nhuận'];

        $scope.search = search;

        $scope.chartdata = [];
        function getStatistic() {
            var config = {
                param: {                  
                    fromDate: $scope.fromDate,
                    toDate: $scope.toDate
                }
            }
            apiService.get('/api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.tabledata = response.data;
                if (response.data == '') {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                }
                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(response.data, function (i, item) {
                    labels.push($filter('date')(item.Date, 'dd/MM/yyyy'));
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = labels;
            }, function (response) {
                notificationService.displayError(response.data.Message);
               // notificationService.displayError('Không thể tải dữ liệu');
            });
        }

        function search() {
            getStatistic();
        }

        getStatistic();
    }

})(angular.module('myshop.statistics'));