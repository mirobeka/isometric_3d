#import "UnityAppController.h"
#include "AudioPluginInterface.h"

#import <objc/runtime.h>

namespace {

  typedef BOOL (*ApplicationDidFinishLaunchingWithOptionsImplemenation)(UnityAppController* app_controller,
                                                                        SEL selector,
                                                                        UIApplication* application,
                                                                        NSDictionary* launch_options);

  ApplicationDidFinishLaunchingWithOptionsImplemenation OriginalApplicationDidFinishLaunchingWithOptions;

  BOOL ApplicationDidFinishLaunchingWithOptions(UnityAppController* app_controller,
                                                SEL selector,
                                                UIApplication* application,
                                                NSDictionary* launch_options) {
    UnityRegisterAudioPlugin(&UnityGetAudioEffectDefinitions);
    return OriginalApplicationDidFinishLaunchingWithOptions(app_controller, selector, application, launch_options);
  }

  IMP Swizzle(SEL selector, Class swizzled_class, IMP new_implemenation) {
    Method method = class_getInstanceMethod(swizzled_class, selector);
    if (method != nil)
      return class_replaceMethod(swizzled_class, selector, new_implemenation, method_getTypeEncoding(method));

    return nil;
  }

} // anonymous namespace

@interface UnityHelmAppController : UnityAppController

@end

@implementation UnityHelmAppController

+ (void)load {
  OriginalApplicationDidFinishLaunchingWithOptions = (ApplicationDidFinishLaunchingWithOptionsImplemenation)
  Swizzle(@selector(application:didFinishLaunchingWithOptions:),
          [UnityAppController class],
          (IMP)&ApplicationDidFinishLaunchingWithOptions);
}

@end
