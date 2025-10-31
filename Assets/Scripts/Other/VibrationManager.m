#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

void TriggerHapticFeedback(float intensity) {
    if (@available(iOS 13.0, *)) {
        UIImpactFeedbackGenerator *generator;
        if (intensity < 0.3) {
            generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleLight];
        } else if (intensity < 0.7) {
            generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
        } else {
            generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleHeavy];
        }
        [generator prepare];
        [generator impactOccurred];
    }
}
