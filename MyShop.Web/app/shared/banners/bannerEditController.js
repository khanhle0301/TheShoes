(function (app) {
    app.controller('bannerEditController', bannerEditController);

    bannerEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function bannerEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.banner = {};
        $scope.UpdateBanner = UpdateBanner;

        function loadBannerDetail() {
            apiService.get('api/banner/getbyid/' + $stateParams.id, null, function (result) {
                $scope.banner = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateBanner() {
            apiService.put('api/banner/update', $scope.banner,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('banners');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.banner.Image = fileUrl;
                })
            }
            finder.popup();
        }

        loadBannerDetail();
    }

})(angular.module('myshop.banners'));