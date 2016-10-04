(function (app) {
    app.controller('bannerAddController', bannerAddController);

    bannerAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function bannerAddController(apiService, $scope, notificationService, $state) {
        $scope.banner = {
            Status: true
        }

        $scope.AddBanner = AddBanner;

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.banner.Image = fileUrl;
                })
            }
            finder.popup();
        }

        function AddBanner() {
            apiService.post('api/banner/create', $scope.banner,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                    $state.go('banners');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }

})(angular.module('myshop.banners'));