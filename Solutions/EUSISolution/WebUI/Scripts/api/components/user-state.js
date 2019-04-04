    (function() {
	'use strict';

	// #######
	// PRIVATE
	// #######

	var CustomStatus = {
        ready: {
            icon: 'mdi mdi-checkbox-marked-circle-outline',
            desc: 'Готов к контакту',
            text: 'ready'
        },
        away: {
            icon: 'mdi mdi-alarm',
            desc: 'Отсутствует',
            text: 'away'
        },
        dontDisturb: {
            icon: 'mdi mdi-alert-circle-outline',
            desc: 'Не беспокоить',
            text: 'dontDisturb'
        },
        inConversation: {
            icon: 'mdi mdi-phone-in-talk',
            desc: 'Ведет разговор',
            text: 'inConversation'
        },
        disconnected: {
            icon: 'mdi mdi-power',
            desc: 'Отключен',
            text: 'disconnected'
        }
    };

	var parseCustomStatus = function(status) {
		if (status && status.toLowerCase) {
			status = status.toLowerCase();
		}

		switch (status) {
			case 0:
			case '0':
			case 'ready':
				return CustomStatus.ready;
			case 1:
			case '1':
			case 'away':
				return CustomStatus.away;
			case 2:
			case '2':
			case 'dontdisturb':
				return CustomStatus.dontDisturb;
			case 3:
			case '3':
			case 'disconnected':
				return CustomStatus.disconnected;
			case 10:
			case '10':
			case 'inconversation':
				return CustomStatus.inConversation;
		}

		console.error('Cannot parse custom status', status);
		return null;
	};

    // ######
    // EXPORT
    // ######

	pbaAPI.getUserState = function(userId, params) {

		// EXTEND DEFAULT PARAMS
		params = $.extend({
			size: '',
			isOnline: false,
			showDesc: false
		}, params);

		// IF userId IS A NUMBER AND IT'S GREATER THAN 0
		var doBind = userId > 0;

		var status = parseCustomStatus(params.status || 'disconnected');

		// RESOLVE ICON CLASSES
		var iconClasses = [];

		iconClasses.push('user-state-icon');
		iconClasses.push(status.icon);

		if (params.size) {
			iconClasses.push('user-state-icon--' + params.size);
		}

		// RESOLVE STATE DATA ATTRIBUTES
		var datas = ['data-user-state-status="' + status.text + '"'];

		// RENDER 'ONLINE' ATTRIBUTE ONLY FOR REGISTERED USERS
	    datas.push('data-user-state-online="' + params.isOnline + '"');


		// ADD USER ID TO DOM AND UPDATE STATE FROM CLIENT CACHE AND SERVER
		if (doBind) {
			datas.push('data-user-state-id="' + userId + '"');

			window.globalapp.userStateService.updateState();
		}

		// GENERATE ICON HTML
		var iconHtml = '<span class="' + iconClasses.join(' ') + '"></span>';

		// GENERATE TEXT HTML
		var descHtml = !params.showDesc ? '' : '&nbsp;<span class="user-state-desc">' + status.desc + '</span>';

		// RESULT HTML
		return '<span class="user-state" ' + datas.join(' ') + '>' +
					iconHtml +
					descHtml +
				'</span>';
    };

    //sib
    pbaAPI.openUserInfo = function (userId) {
        pbaAPI.proxyclient.corpProp.getUserProfile({
            id: userId
        }).done(function (result) {
            if (result) {
                if (result.ID)
                    pbaAPI.openDetailView('SibUser', {
                        id: result.ID,
                        callback: function (e) {
                            corpProp.dv.wnd.delWnd("SibUser");
                        }
                    });
            }
        });
    };


	pbaAPI.setProfileInfo = function (userId) {  
	    pbaAPI.proxyclient.corpProp.getUserProfile({	       
	        id: userId
	    }).done(function (result) {
	        if (result) {
                $("#sib-profile-dept").html(result.SocietyDeptName);
	            $("#sib-profile-society").html(result.SocietyName);
	        }
	    });
    };
    //end sib
}());
