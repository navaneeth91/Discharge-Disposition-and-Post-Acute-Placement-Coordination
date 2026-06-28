import { defineStore } from "pinia";
import { startSignalR } from "@/services/signalRService";
import { useToast } from "vue-toastification";
import router from "@/router";

export const useNotificationStore =
defineStore("notification", {

    state: () => ({
        notifications: [],
        unreadCount: 0,
        connection: null
    }),

    actions: {

        async initialize(token) {

            const toast = useToast();

            this.connection =
                await startSignalR(token);

            this.connection.on(
                "ReceiveNotification",

                (notification) => {

                    this.notifications.unshift(notification);
                    this.unreadCount++;
                    toast.success(notification.message);

                });

            this.connection.on(
                "RefreshDashboard",

                () => {

                    window.dispatchEvent(
                        new Event("refresh-dashboard"));

                });

            this.connection.on(
                "RefreshAssignments",

                () => {

                    window.dispatchEvent(
                        new Event("refresh-assignments"));

                });
                this.connection.on(
                    "RefreshReferrals",

                    () => {

                        window.dispatchEvent(
                            new Event("refresh-referrals"));

                    });
                    this.connection.on(
                        "RefreshAuthorizations",

                        () => {

                            window.dispatchEvent(
                                new Event("refresh-authorizations"));

                        });
        },
        clearUnread() {

        this.unreadCount = 0;

    }
    }
});