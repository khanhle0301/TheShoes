(function (app) {
    app.controller('vendorAddController', vendorAddController);

    vendorAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function vendorAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.vendor = {
            CreatedDate: new Date(),
            Status: true
        }

        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.vendor.Alias = commonService.getSeoTitle($scope.vendor.Name);
        }

        $scope.AddVendor = AddVendor;

        function AddVendor() {
            apiService.post('api/vendor/create', $scope.vendor,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                    $state.go('vendors');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.vendor.Image = fileUrl;
                })
            }
            finder.popup();
        }       
    }

})(angular.module('myshop.vendors'));