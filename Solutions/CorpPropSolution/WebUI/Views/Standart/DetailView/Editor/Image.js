pbaAPI.registerEditor('Image', pbaAPI.Editor.extend({
    $uploadCrop: null,
    tempFilename: null,
    rawImg: null,
    modalWnd: null,
    defImage: 'NoImage',
    needCrop: true,
    //CUSTOM
    readfile: function (file) {
        var self = this;
        var reader = new FileReader();
        reader.onload = function (e) {
            self.rawImg = e.target.result;
            self.openCropWindow();
        };

        if (self.needCrop) {
            reader.readAsDataURL(file);
        } else {            
            self.setImageWithoutCropping(file);
        }
    },
    setImageWithoutCropping: function (file) {
        var self = this;        
        var formdata = new FormData();
        formdata.append(file.FileName, file);
        $.ajax({
            processData: false,
            contentType: false,
            type: "POST",
            url: "/FileData/SaveFile",
            data: formdata,
            success: function (res) {                
                var fileLink = pbaAPI.imageHelpers.getImageThumbnailSrc(res, 'S', self.defImage, 'frame');
                self.pbaForm.setPr(self.propertyName, res);
                self.wrap.find('#item-img-output').attr('src', fileLink);
            }
        });
    },
    openCropWindow: function () {
        var self = this;
        if (!self.modalWnd) {
            self.modalWnd = self.wrap.find('#cropImagePop').kendoWindow({
                width: 500,
                height: 550,
                resizable: false,
                modal: true,
                activate: function () {                    
                    if (!self.$uploadCrop) {
                        var $uploadDemo = this.element.find('#upload-demo');
                        var cropWidth = $uploadDemo.data('cropwidth');
                        var cropHeight = $uploadDemo.data('cropheight');
                        var shapeType = $uploadDemo.data('shapetype');
                        self.$uploadCrop = $uploadDemo.croppie({
                            viewport: {
                                width: cropWidth,
                                height: cropHeight,
                                type: shapeType
                            }
                        });
                    }
                    self.$uploadCrop.croppie('bind', {
                        url: self.rawImg
                    }).then(function () {                        
                    });
                }
            }).data('kendoWindow');
        }
        self.modalWnd.open().center();
        self.modalWnd.wrapper.addClass('image-crop-modal');
    },
    onAfterBind: function () {
        var self = this;
        var file = self.pbaForm.getPr(self.propertyName);

        var link = pbaAPI.imageHelpers.getImageThumbnailSrc(file, 'S', self.defImage, 'frame');
  
        self.wrap.find('#item-img-output').attr('src', link);

        self.wrap.find('#item-img-output')                
                .off().on('click', function () {
                    var curFile = self.pbaForm.getPr(self.propertyName);
                    if (curFile != null) {
                        pbaAPI.showImage(curFile.FileID, curFile.FileName);
                    }                    
                });
    },
    init: function ($wrap, propertyName) {
        'use strict';

        pbaAPI.Editor.fn.init.call(this, $wrap, propertyName);
        var self = this;
        self.needCrop = $wrap.data('needcrop') === 'True' ? true : false;
        self.defImage = $wrap.data('defaultimage');
        if ($wrap.find('.item-img')) {
            $wrap.find('.item-img').on('change', function () {                
                self.tempFilename = $(this).val();
                if (this.files && this.files[0]) {
                    self.readfile(this.files[0]);
                }
            });
            $wrap.find('.item-img').on('click', function () {
                this.value = null;
            });            
        }
        if ($wrap.find('#choose_img_btn')) {
            $wrap.find('#choose_img_btn').click(function (e) {
                pbaAPI.openModalDialog('FileStorageItemImage', function (items) {
                    if (!items || items.length === 0)
                        return pbaAPI.errorMsg('Ошибка при выборе изображения');
                    var file = items[0].File;
                    if (self.needCrop) {
                        self.pbaForm.setPr(self.propertyName, { ID: file.ID, FileID: file.FileID });
                        var fileLink = pbaAPI.imageHelpers.getImageThumbnailSrc(file, 'S', self.defImage, 'frame');
                        self.rawImg = fileLink;
                        self.openCropWindow();
                    } else {
                        self.pbaForm.setPr(self.propertyName, { ID: file.ID, FileID: file.FileID });
                        var fileLink = pbaAPI.imageHelpers.getImageThumbnailSrc(file, 'S', self.defImage, 'frame');
                        $wrap.find('#item-img-output').attr('src', fileLink);
                    }
                });
            });
        }

        self.wrap.find('#delete_img_btn').click(function (e) {
            self.pbaForm.setPr(self.propertyName, null);
            var defaultImg = pbaAPI.imageHelpers.getImageThumbnailSrc(null, 'S', self.defImage, 'frame');
            self.wrap.find('#item-img-output').attr('src', defaultImg);
        });

        $wrap.find('#cropImageBtn').on('click', function (ev) {
            var cropWidth = self.$uploadCrop.data('cropwidth');
            var cropHeight = self.$uploadCrop.data('cropheight');
            var cropSize = {
                width: cropWidth,
                height: cropHeight
            };
            self.$uploadCrop.croppie('result', {
                type: 'blob',
                format: 'png',
                size: cropSize
            }).then(function (resp) {
                var formdata = new FormData();
                resp.lastModifiedDate = new Date();
                resp.name = self.tempFilename;
                formdata.append(self.tempFilename, resp);
                $.ajax({
                    processData: false,
                    contentType: false,
                    type: "POST",
                    url: "/FileData/SaveFile",
                    data: formdata,
                    success: function (res) {
                        var fileLink = pbaAPI.imageHelpers.getImageThumbnailSrc(res, 'S', self.defImage, 'frame');
                        self.pbaForm.setPr(self.propertyName, res);
                        $wrap.find('#item-img-output').attr('src', fileLink);
                        self.modalWnd.close();
                    }
                });
            });
        });
    }
}));