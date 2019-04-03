(function(angular, globalApp, pbaAPI) {
	'use strict';

	var messageController = function($scope, $sce, $element, FileUploader, DialogService, MessageService, RouteService, UserService) {
        
		// ############
		// DEPENDENCIES
		// ############
		
		var rtcService = globalApp.rtcService;

		// #####
		// STATE
		// #####

        var dialogsLoaded = false;
		var messagesLoaded = false;
        var dialogType, dialogId;
        
        // MULTIMEDIA UPLOADER HELPER DATA
		var expectedCount = 0;
	    var lastRecordType = '';
	    var uploadedMutimedia = [];
        
		// #######
		// HELPERS
		// #######
        
        var isCurrentlyOpened = function(dialog) {
            var page = RouteService.getPage();
            var state = RouteService.getState();
            
            return page === 'messages' && state.dialogType === dialog.type && state.dialogId === dialog.id;
        };

		var updateState = function() {
			var prevState = {
				dialog: $scope.dialog,
				isLoaded: $scope.isLoaded,
				messages: $scope.messages,
			};

			$scope.dialog = DialogService.getDialog(dialogType, dialogId);

			dialogsLoaded = DialogService.isLoaded;
			messagesLoaded = MessageService.isLoaded($scope.dialog);

			$scope.isLoaded = dialogsLoaded && messagesLoaded;
			$scope.messages = MessageService.getMessages($scope.dialog);

			var requireApply = prevState.dialog !== $scope.dialog || prevState.isLoaded !== $scope.isLoaded || prevState.messages !== $scope.messages;

			return requireApply;
		};

		var scrollToBottom = function() {
			if (!messagesLoaded) return;

			setTimeout(function() {
				var $wrapper = $element.find('.chat__messages .ps-container');
                var $container = $wrapper.children('[ng-transclude]');
                
                var lastScroll = scrollToBottom._lastScroll || 0;
                var scrollTo = $container.height() - $wrapper.height();
                if (scrollTo < 0) {
                    scrollTo = 0;
                }
                
                if (scrollTo < lastScroll) {
                    $wrapper.scrollTop(0);
                }
                
                if (scrollTo > 0) {
                    $wrapper.stop().animate({ scrollTop: scrollTo }, 'slow');
                }
                
                scrollToBottom._lastScroll = scrollTo;
			}, 0);
		};

		var renderSize = function(bytes) {
			var tags = ['байт', 'Кбайт', 'Мбайт', 'Гбайт', 'Тбайт'];
			var step = 1024;
			var index = 0;

			while (bytes > 1024 && index < tags.length - 1) {
				bytes /= step;
				index++;
			}

			return bytes.toFixed(2) + ' ' + tags[index];
		};

		var call = function(sessionType) {
		    var type;

		    switch (dialogType) {
		        case 'private':
		            type = 'PrivateMessage';
		            break;
		        case 'conference':
		            type = 'PublicMessage';
		            break;
		        default: return;
		    }

		    rtcService.sendCallRequest(dialogId, type, null, sessionType);
		};

		var record = function(type) {

		    var msgCtrl = this;

		    switch (type) {
		        case 'audio':
		            lastRecordType = 0;
		            break;
		        case 'video':
		            lastRecordType = 1;
		            break;
		        default:
		            lastRecordType = 2;
		            break;
		    }

		    rtcService.startRecording(type, {

		        onstop: function(blobs) {

		            (blobs || []).forEach(function(blob) {
		                var name;

		                switch (blob.type) {
		                    case 'audio/wav':
		                    case 'audio/ogg':
		                    case 'video/webm':
		                        name = 'offline.' + blob.type.split('/')[1];
		                        break;
		                    default:
		                        name = "offline.unk";
		                        break;
		                }

		                msgCtrl.multimediaUploader.addToQueue(new File([blob], name));
		            });

		            var notUploadedCount = msgCtrl.multimediaUploader.getNotUploadedItems().length;

		            if (notUploadedCount) {
		                expectedCount = notUploadedCount;
		                uploadedMutimedia = [];
		                msgCtrl.multimediaUploader.uploadAll();
		            }
		        }

		    });

		}.bind(this);

		var toTimeFormat = function(number) {
		    if (number < 10) return '0' + number;
		    return number + '';
		};

		// ############
		// VIEW HELPERS
		// ############

        this.getMemberImageHtml = function(member, isMaximized) {
            if (!member) {
                return '';
            }

            var html = UserService.getLogo({
               ID: member.id,
               Image: { FileID: member.imageId },
               FullName: member.title
            }, !isMaximized, 60, 60, true, 'med', true);
            
            return $sce.trustAsHtml('<div data-mnemonic="User" data-id="' + member.id + '">' + html + '</div>');
        };

		this.getDialogImageHtml = function(dialog) {
            if (!dialog) {
                return '';
            }
            
            if (dialog.type === 'conference') {
                return $sce.trustAsHtml(dialog.title);
            }
            
            var html = UserService.getLogo({
                ID: dialog.id,
                Image: { FileID: dialog.imageId },
                FullName: dialog.title, 
            }, false, 60, 60, true, 'small', true);
            
            return $sce.trustAsHtml(html);
		};

		this.getMessageImageHtml = function(message) {
			var html = UserService.getLogo({
				ID: message.fromId,
				Image: { FileID: message.fromImageId }
			}, true, 60, 60, false);

			return $sce.trustAsHtml(html);
		};

		this.getMembers = function() {
			return DialogService.getMembers($scope.dialog);
		};

		this.getUserName = function(message) {
			if (!message || !message.isConference || !message.isIncoming) {
				return '';
			}

			var member = DialogService.getMembers($scope.dialog)
				.filter(function(member) { return member.id === message.fromId; })[0];

			return member ? member.title : '';
		};

		// #########################
		// CONFERENCE MEMBER ACTIONS
		// #########################
		
		this.gotoPrivate = function(member) {
			RouteService.goTo.newDialog('private', member.id);
		};

		this.renderFile = function(message) {
			if (!message || !message.file) {
				return null;
			}

			var builder = [];

			var preview = pbaAPI.getFilePreviewHtml(message.file.orig, 50, 50);

			if (message.type === 'presentation') {
				preview += '<a class="chat__message-attachment-play" onclick="pbaAPI.showPresentation(\'' + message.file.id + '\')" ontouchstart="pbaAPI.showPresentation(\'' + message.file.id + '\')"><span class="glyphicon glyphicon-play"></span></a>';
			}

			builder.push('<span class="chat__message-file-icon chat__message-attachment">' + preview + '</span>');
			builder.push('<h4 class="chat__message-file-title text-wrap">' + (message.file.title || 'noname') + '</h4>');
			builder.push('<p class="chat__message-file-size text-wrap">' + renderSize(message.file.size) + '</p>');

			if (preview.indexOf('img')) {
				builder[0] = builder[0].replace(/(img.+class=")(file-icon)(")/, '$1$2 chat__message-attachment-item$3');
			}

			return $sce.trustAsHtml(builder.join(''));
		};

		this.renderMultimedia = function(message) {
			if (!message || message.type !== 'multimedia') {
				return null;
			}

			var builder = [];

			if (message.multimediaType === 'video') {
				builder.push('<video class="chat__message-attachment chat__message-attachment-item" src="/multimedia/getmedia?id=' + message.multimediaId + '" width="250" height="188" controls></video>');
			} else if (message.multimediaType === 'audio') {
				builder.push('<audio class="chat__message-attachment chat__message-attachment-item" src="/multimedia/getmedia?id=' + message.multimediaId + '" controls></audio>');
			}

			builder.push('<div class="clearfix-block"></div>')

			return $sce.trustAsHtml(builder.join(''));
		};

		this.printDateTime = function(message) {
		    var d = message.date;

		    if (!d) {
		        return '--:--';
		    }

		    var date = [d.getDate(), d.getMonth()]
		        .map(toTimeFormat)
		        .concat([d.getFullYear()])
		        .join('.');

		    var time = [d.getHours(), d.getMinutes(), d.getSeconds()]
		        .map(toTimeFormat)
		        .join(':');

		    return date + ' ' + time;
		};

		this.printTime = function(message) {
			var d = message.date;

			if (!d) return '--:--';

			var time = [d.getHours(), d.getMinutes()]
                .map(toTimeFormat)
                .join(':');

			return time;
		};

	    this.onSubmit = function() {
			var form = document.getElementById('newMessageForm');
            if (!form) return;

            var input = form.newMessageText;
            if (!input) return;

            var text = input.value.trim()/*.replace(/\s+/, ' ')*/;
            if (!text) return;

            input.value = '';

            MessageService.sendTextMessage($scope.dialog, text);
		};

		this.onEnterPress = function(event) {
			event.preventDefault();

			if (!event.ctrl) {
				this.onSubmit();
				return;
			}

			var form = document.getElementById('newMessageForm');
            if (!form) return;

            var input = form.newMessageText;
            if (!input) return;

            input.value += '\n';

            $(input).scrollTop(input.scrollHeight);
		};

		this.audioCall = function() {
		    call('audio');
		};

		this.videoCall = function() {
		    call('video');
		};

		this.recordAudio = function() {
			record('audio');
		};

		this.recordVideo = function() {
			record('video');
		};

		this.isConference = function() {
			return dialogType === 'conference';
		};

        // Remove create from private dialog feature
		this.createConference = function() {
			var isConference = this.isConference();

			// RESOLVE USER IDS THAT SOULDN'T BE SHOWN
			var ignoreIds = isConference
				? pbaAPI.extract(this.getMembers(), 'id')	// conference members ids
				: [dialogId];								// current dialog (user) id

			// COMPOSE SYSTEM FILTER
			var filter = ignoreIds
				.map(function(id) { return 'it.ID!=' + id; }) // from each dialog id
				.concat(['it.ID!=@CurrentUserID'])
				.join(' and ');

			pbaAPI.openModalDialog('User', function(selectedUsers) {

				// IF NO USER HAS BEEN SELECTED
				if (!selectedUsers.length) return;

				// EXTRACT USER IDS
				var selectedIds = pbaAPI.extract(selectedUsers, 'ID');

				if (isConference) {

					// ADD USER(S) TO EXISTENT CONFERENCE
					DialogService.inviteToConference(dialogId, selectedIds);
				} else {

					// CREATE NEW CONFERENCE WITH SELECTED USERS
					DialogService.createConference(selectedIds.concat([dialogId]));
				}

			}, {
				title: isConference ? 'Добавление пользователей к конференции' : 'Создание конференции (выбор пользователей)',
				filter: filter,
				multiselect: true,
			});
		};

		this.excludeConferenceMember = function(member) {
			DialogService.excludeFromConference(dialogId, member.id);
		};

		this.leaveConference = function() {
            // unused ???
			//var isConference = this.isConference();

			pbaAPI.confirm('Предупреждение', 'Вы действительно хотите покинуть эту конференцию?', function() {
				DialogService.excludeFromConference(dialogId, UserService.current.id);
			});
		};

	    // MULTI-FILE (MAX IS 2) UPLOADER (MULTIMEDIA MESSAGES)
		this.multimediaUploader = new FileUploader({
		    url: '/filedata/savefiles',
		    autoUpload: true,
		    removeAfterUpload: true,
		    queueLimit: 2,
		    onSuccessItem: function (item, response, status, headers) {

		    	// EXPECTED, THAT response IS AN ARRAY WITH ONE FILE ELEMENT
		        var file = response ? response[0] : null;

		        if (!$scope.dialog || !file) return;

		        // SAVE THIS UPLOADED FILE FOR A MULTIMEDIA MESSAGE
		        uploadedMutimedia.push(file);

		        // IT'S THE LAST FILE IN UPLOAD QUEUE - SEND MESSAGE
		        if (uploadedMutimedia.length === expectedCount) {
		            MessageService.sendMultimediaMessage($scope.dialog, uploadedMutimedia, lastRecordType);
		        }
		    }
		});

		// SINGLE-FILE UPLOADER (FILE MESSAGES)
		this.uploader = new FileUploader({
			url: '/filedata/savefiles',
			autoUpload: true,
			removeAfterUpload: true,
            queueLimit: 1,
			onSuccessItem: function(item, response, status, headers) {

				// EXPECTED, THAT response IS AN ARRAY WITH ONE FILE ELEMENT
				var file = response ? response[0] : null;

				if (!$scope.dialog || !file) return;

				// SEND FILE MESSAGE
				MessageService.sendFileMessage($scope.dialog, file);
			}
		});

		this.uploadingProgress = function() {
		    return this.uploader.isUploading ? this.uploader.progress : 0;
		};

		this.cancelUploading = function() {
		    if (this.uploader.isUploading) {
		        this.uploader.cancelAll();
		        this.uploader.clearQueue();
		    }
		};

		this.uploadFile = function() {
		    $element.find('input[type="file"]').trigger('click');
		};

        // ####################
        // ON CONTROLLER CREATE
        // ####################

		RouteService.onRouteChange(function(page, state) {
            if (page === 'messages') {
                dialogType = state.dialogType;
                dialogId = state.dialogId;

                if (updateState()) {
                    $scope.$applyAsync();
                }

                if (dialogsLoaded) {
                    if (!$scope.dialog) {
                        RouteService.goTo.dialogs();
                    } else if (!messagesLoaded) {
                        MessageService.load($scope.dialog);
                    } else {
                        MessageService.readMessages($scope.dialog);
                    }
                }
                
                scrollToBottom();
            }
        });

		// #################
		// LISTEN TO SOCKETS
		// #################

		DialogService.onUpdate(function() {
			if (updateState()) {
                $scope.$applyAsync();
            }

			if (dialogsLoaded) {
				if (!$scope.dialog) {
					RouteService.goTo.dialogs();
				} else if (!messagesLoaded) {
					MessageService.load($scope.dialog);
				}/* else {
					MessageService.readMessages($scope.dialog);
				}*/
			}
            
            scrollToBottom();
		});
        
		MessageService.onUpdate(function(dialog) {
			if (!isCurrentlyOpened(dialog)) {
                return;
            }

            // UPDATE MESSAGE CTRL SCOPE
            updateState();
            
            // APPLY CHANGES
            $scope.$applyAsync();
            
            // SEND REQUEST TO THE SERVER TO AUTO-READ ALL MESSAGES FROM THIS DIALOG
            MessageService.readMessages(dialog);
            
            // SCROLL VIEW TO BOTTOM
            scrollToBottom();
		});

		MessageService.onNewMessage(function(dialog, message) {
            if (!isCurrentlyOpened(dialog)) {
                return;
            }

            // UPDATE MESSAGE CTRL SCOPE
            updateState();
            
            // APPLY CHANGES
            $scope.$applyAsync();
            
            // SEND REQUEST TO THE SERVER TO AUTO-READ ALL MESSAGES FROM THIS DIALOG
            MessageService.readMessages(dialog);
            
            // SCROLL VIEW TO BOTTOM
            scrollToBottom();
		});

		// #####################
		// ON CONTROLLER DESTROY
		// #####################
		
		// $scope.$on('$destroy', function() {
		// 	DialogService.releaseControllers();
		// 	MessageService.releaseControllers();
		// });
	};

	angular.module('ChatApp.Controllers')
		.controller('MessageController', messageController);

	messageController.$inject = ['$scope', '$sce', '$element', 'FileUploader', 'DialogService', 'MessageService', 'RouteService', 'UserService'];

}(window.angular, window.globalapp, window.pbaAPI));