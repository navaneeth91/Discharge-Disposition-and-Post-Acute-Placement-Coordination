<template>
  <div class="min-h-screen flex overflow-hidden">

    <!-- Left Panel -->

    <div
      class="
      hidden
      lg:flex
      w-1/2
      relative
      overflow-hidden
      bg-gradient-to-br
      from-[#003049]
      via-[#0B476A]
      to-[#669BBC]
      text-white">

      <!-- Background Shapes -->

      <div class="absolute inset-0 opacity-20">

        <div
          class="
          absolute
          top-16
          left-16
          h-72
          w-72
          rounded-full
          bg-white
          blur-3xl">
        </div>

        <div
          class="
          absolute
          bottom-10
          right-10
          h-64
          w-64
          rounded-full
          bg-purple-200
          blur-3xl">
        </div>

      </div>

      <div
        class="
        relative
        z-10
        flex
        flex-col
        justify-center
        px-20">

        <!-- Logo -->

        <div
          class="
          floating
          mb-10
          h-20
          w-20
          rounded-3xl
          bg-white/20
          backdrop-blur-md
          flex
          items-center
          justify-center
          text-4xl
          font-bold
          shadow-2xl">

          H

        </div>

        <!-- Title -->

        <h1
          class="
          text-6xl
          font-bold
          leading-tight
          mb-8">

          Discharge
          <br>
          Disposition
          <br>
          System

        </h1>

        <!-- Description -->

        <p
          class="
          max-w-lg
          text-xl
          text-blue-100
          leading-relaxed">

          Streamlining patient discharge planning,
          insurance authorization and post-acute
          placement coordination with an integrated
          healthcare platform.

        </p>

      </div>

    </div>

    <!-- Right Side -->

    <div
      class="
      flex-1
      flex
      items-center
      justify-center
      bg-[#FDF0D5]
      p-8">

      <slot />

    </div>

  </div>
</template>