(function (app) {
    app.controller('vendorEditController', vendorEditController);

    vendorEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];

    function vendorEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.vendor = {
            CreatedDate: new Date(),
            Status: true
        }

        $scope.UpdateVendor = UpdateVendor;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.vendor.Alias = commonService.getSeoTitle($scope.vendor.Name);
        }

        function loadVendorDetail() {
            apiService.get('api/vendor/getbyid/' + $stateParams.id, null, function (result) {
                $scope.vendor = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
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

        function UpdateVendor() {
            apiService.put('api/vendor/update', $scope.vendor,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('vendors');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
       
        loadVendorDetail();
    }

})(angular.module('myshop.vendors'));