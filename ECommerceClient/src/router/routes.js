const routes = [
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      {
        path: "/",
        component: () => import("pages/HomePage.vue"),
      },
      {
        path: "/brands",
        component: () => import("pages/BrandList.vue"),
      },
      {
        path: "/cart",
        component: () => import("pages/CartPage.vue"),
      },
      {
        path: "/register",
        component: () => import("pages/RegisterPage.vue"),
      },
      {
        path: "/login",
        component: () => import("pages/LoginPage.vue"),
      },
      {
        path: "/logout",
        component: () => import("pages/LogoutPage.vue"),
      },
      {
        path: "/orders",
        component: () => import("pages/OrderHistoryPage.vue"),
      },
      {
        path: "/maps",
        component: () => import("pages/BranchLocator.vue"),
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];

export default routes;
