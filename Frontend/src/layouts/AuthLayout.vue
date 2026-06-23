<template>
  <div
    class="min-h-screen flex bg-slate-50"
  >
    <!-- Left Side -->

    <div
      class="hidden lg:flex w-1/2 bg-gradient-to-br from-blue-700 via-blue-600 to-cyan-500 text-white p-12 flex-col justify-center"
    >
      <h1 class="text-5xl font-bold mb-6">
        Discharge Disposition System
      </h1>

      <p class="text-xl opacity-90 leading-relaxed">
        Streamline patient discharge planning,
        authorization management and
        post-acute placement coordination.
      </p>
    </div>

    <!-- Right Side -->

    <div
      class="flex-1 flex justify-center items-center p-8"
    >
      <slot />
    </div>
  </div>
</template>